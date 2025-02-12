using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Create;

public sealed class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<CreateVolunteerCommand> _validator;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository, IValidator<CreateVolunteerCommand> validator, ILogger<CreateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ValidationResult>> Handle(CreateVolunteerCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult;
        }
        
        var volunteerId = VolunteerId.NewVolunteerId();
        
        var fullName = FullName.Create(command.FirstName, command.LastName, command.MiddleName).Value;
        var email = Email.Create(command.Email).Value;

        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        
        var requisites = command.Requisites?.Select(r=> Requisite.Create(r.Name, r.Description).Value).ToList();

        var socialMedias = command.SocialMedias?.Select(r=> SocialMedia.Create(r.Name, r.Link).Value).ToList();
        
        var volunteer = new Volunteer(volunteerId, fullName, email, phoneNumber);

        if (requisites != null && requisites.Count != 0)
        {
            volunteer.UpdateRequisites(requisites);
        }

        if (socialMedias != null && socialMedias.Count != 0)
        {
            volunteer.UpdateSocialMedias(socialMedias);
        }

        await _volunteersRepository.Add(volunteer, cancellationToken);
        
        _logger.LogInformation("Created volunteer with id: {volunteerId}", volunteer.Id.Value);

        return (Guid)volunteer.Id;
    }
}