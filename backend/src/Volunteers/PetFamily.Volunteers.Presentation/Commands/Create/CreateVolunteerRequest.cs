namespace PetFamily.Volunteers.Presentation.Commands.Create;

public record CreateVolunteerRequest(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string PhoneNumber,
    IEnumerable<RequisiteDto>? Requisites,
    IEnumerable<SocialMediaDto>? SocialMedias);
