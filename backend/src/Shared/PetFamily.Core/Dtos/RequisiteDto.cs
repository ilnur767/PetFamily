namespace PetFamily.Core.Dtos;

public class RequisiteDto
{
    public RequisiteDto(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; set; }
    public string Description { get; set; }
}
