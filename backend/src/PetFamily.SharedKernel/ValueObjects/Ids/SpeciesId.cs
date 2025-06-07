using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects.Ids;

public class SpeciesId : ComparableValueObject
{
    private SpeciesId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static SpeciesId NewSpeciesId()
    {
        return new SpeciesId(Guid.NewGuid());
    }

    public static SpeciesId Empty()
    {
        return new SpeciesId(Guid.Empty);
    }

    public static SpeciesId Create(Guid value)
    {
        return new SpeciesId(value);
    }

    public static implicit operator Guid(SpeciesId speciesId)
    {
        ArgumentNullException.ThrowIfNull(speciesId);

        return speciesId.Value;
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}
