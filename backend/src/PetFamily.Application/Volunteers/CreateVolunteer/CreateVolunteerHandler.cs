using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

[UsedImplicitly]
public sealed class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    
    public async Task<Result<Guid, Error>> Handle(CreateVolunteerCommand command, CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        
        var fullName = FullName.Create(command.FirstName, command.LastName, command.MiddleName);
        if (fullName.IsFailure)
        {
            return fullName.Error;
        }
        
        var email = Email.Create(command.Email);
        if (email.IsFailure)
        {
            return email.Error;
        }
        
        var phoneNumber= PhoneNumber.Create(command.PhoneNumber);
        if (phoneNumber.IsFailure)
        {
            return phoneNumber.Error;
        }
        
        var requisites = command.Requisites?.Select(r=> Requisite.Create(r.Name, r.Description));
        
        if (requisites != null && requisites.Any(r=> r.IsFailure))
        {
            return requisites.First(r=>r.IsFailure).Error;
        }
        
        var socialMedias = command.SocialMediaCommands?.Select(r=> SocialMedia.Create(r.Name, r.Link));
        
        if (socialMedias != null && socialMedias.Any(r=> r.IsFailure))
        {
            return socialMedias.First(r=>r.IsFailure).Error;
        }
        
        var volunteer = Volunteer.Create(volunteerId, fullName.Value, email.Value, phoneNumber.Value);
        if (volunteer.IsFailure)
        {
            return volunteer.Error;
        }
        
        volunteer.Value.AddRequisites(requisites.Select(r=>r.Value));
        volunteer.Value.AddSocialMedias(socialMedias.Select(r=>r.Value));
        
        await _volunteersRepository.Add(volunteer.Value, cancellationToken);
        
        return (Guid)volunteer.Value.Id;
    }
}