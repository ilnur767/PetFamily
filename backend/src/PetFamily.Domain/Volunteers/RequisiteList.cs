using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteers;

public class RequisiteList : ComparableValueObject
{
    public RequisiteList() { }
    
    private RequisiteList(List<Requisite> requisites)
    {
        Requisites = requisites;
    }

    public static RequisiteList Create(List<Requisite> requisites)
    {
        return new RequisiteList(requisites);
    }

    public List<Requisite> Requisites { get; }
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        foreach (var requisite in Requisites)
        {
            yield return requisite;
        }
    }
}