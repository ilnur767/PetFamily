namespace PetFamily.Domain.Volunteers;

public record RequisiteList
{
    public List<Requisite> Requisites { get; private set; }
}