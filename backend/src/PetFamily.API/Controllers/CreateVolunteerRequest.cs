namespace PetFamily.API.Controllers;

public record CreateVolunteerRequest(string FirstName, string LastName, string MiddleName, string Email, string PhoneNumber, IEnumerable<RequisiteDto>? Requisites, IEnumerable<SocialMediaDto>? SocialMedias);