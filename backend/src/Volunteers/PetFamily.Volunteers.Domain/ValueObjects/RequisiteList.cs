using CSharpFunctionalExtensions;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public class RequisiteList : ComparableValueObject
{
    public RequisiteList()
    {
    }

    private RequisiteList(List<Requisite> requisites)
    {
        Requisites = requisites;
    }

    public List<Requisite> Requisites { get; }

    public static RequisiteList Create(List<Requisite> requisites)
    {
        return new RequisiteList(requisites);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        foreach (var requisite in Requisites)
        {
            yield return requisite;
        }
    }
}
