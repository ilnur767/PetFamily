using CSharpFunctionalExtensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Common;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Domain.Entities;

namespace PetFamily.Volunteers.Application.Commands.Create;

[UsedImplicitly]
public sealed class CreateVolunteerHandler : ICommandHandler<Guid, CreateVolunteerCommand>
{
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private readonly IValidator<CreateVolunteerCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository, IValidator<CreateVolunteerCommand> validator,
        ILogger<CreateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateVolunteerCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorList();
        }

        var volunteerId = VolunteerId.NewVolunteerId();

        var fullName = FullName.Create(command.FirstName, command.LastName, command.MiddleName).Value;
        var email = Email.Create(command.Email).Value;

        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var volunteer = new Volunteer(volunteerId, fullName, email, phoneNumber);

        await _volunteersRepository.Add(volunteer, cancellationToken);

        _logger.LogInformation("Created volunteer with id: {volunteerId}", volunteer.Id.Value);

        return (Guid)volunteer.Id;
    }
}
