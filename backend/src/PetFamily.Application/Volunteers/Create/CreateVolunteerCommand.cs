namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerCommand(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string PhoneNumber,
    IEnumerable<CreateRequisiteCommand>? Requisites,
    IEnumerable<CreateSocialMediaCommand>? SocialMedias);