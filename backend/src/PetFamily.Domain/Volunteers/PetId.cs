using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteers;

public class PetId : ComparableValueObject
{
    private PetId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static PetId NewPetId()
    {
        return new PetId(Guid.NewGuid());
    }

    public static PetId Empty()
    {
        return new PetId(Guid.Empty);
    }

    public static PetId Create(Guid value)
    {
        return new PetId(value);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}