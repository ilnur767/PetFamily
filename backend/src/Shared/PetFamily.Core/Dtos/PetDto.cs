using System.Text.Json.Serialization;

namespace PetFamily.Core.Dtos;

public class PetDto
{
    public Guid Id { get; set; }
    public string NickName { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public string? HealthInformation { get; set; }
    public string? Address { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsCastrated { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool? IsVaccinated { get; set; }
    public Guid SpeciesId { get; set; }
    public Guid BreedId { get; set; }
    public PhotoDto[]? Photos { get; set; }
    public RequisiteDto[]? Requisites { get; set; }
    public Guid VolunteerId { get; set; }
    [JsonIgnore] public bool IsDeleted { get; set; }
}
