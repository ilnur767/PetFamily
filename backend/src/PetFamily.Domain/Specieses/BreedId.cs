using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Specieses;

public class BreedId : ComparableValueObject
{
    private BreedId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static BreedId NewBreedId()
    {
        return new BreedId(Guid.NewGuid());
    }

    public static BreedId Empty()
    {
        return new BreedId(Guid.Empty);
    }

    public static BreedId Create(Guid value)
    {
        return new BreedId(value);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}