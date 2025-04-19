using System.Text.Json.Serialization;

namespace PetFamily.Application.Dtos;

public class VolunteerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? WorkExperience { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonIgnore]
    public bool IsDeleted { get; set; }
}
