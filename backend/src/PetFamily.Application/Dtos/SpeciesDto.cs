using System.Text.Json.Serialization;

namespace PetFamily.Application.Dtos;

public class SpeciesDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public bool IsDeleted { get; set; }
}

public class BreedDto
{
    public Guid Id { get; set; }
    public Guid SpeciesId { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public bool IsDeleted { get; set; }
}
