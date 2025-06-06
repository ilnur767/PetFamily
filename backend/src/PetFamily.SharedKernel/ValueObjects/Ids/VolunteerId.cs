using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects.Ids;

public class VolunteerId : ComparableValueObject
{
    private VolunteerId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static VolunteerId NewVolunteerId()
    {
        return new VolunteerId(Guid.NewGuid());
    }

    public static VolunteerId Empty()
    {
        return new VolunteerId(Guid.Empty);
    }

    public static VolunteerId Create(Guid value)
    {
        return new VolunteerId(value);
    }

    public static implicit operator Guid(VolunteerId volunteerId)
    {
        ArgumentNullException.ThrowIfNull(volunteerId);

        return volunteerId.Value;
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}
