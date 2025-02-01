namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerCommand(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string PhoneNumber,
    IEnumerable<CreateRequisiteCommand>? Requisites,
    IEnumerable<CreateSocialMediaCommand>? SocialMedias);