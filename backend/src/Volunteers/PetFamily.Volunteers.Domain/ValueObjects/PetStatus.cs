using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public enum PetStatusValue
{
    NeedsHelp,
    LookingForHome,
    FoundHome
}

public class PetStatus : ComparableValueObject
{
    private PetStatus(PetStatusValue status)
    {
        Status = status;
    }

    public PetStatusValue Status { get; }

    public static Result<PetStatus, Error> Create(string petStatus)
    {
        if (string.IsNullOrWhiteSpace(petStatus))
        {
            return Errors.General.ValueIsRequired(nameof(petStatus));
        }

        petStatus = petStatus.Trim();

        var parseResult = Enum.TryParse(petStatus, true, out PetStatusValue petStatusValue);
        if (parseResult == false)
        {
            return Errors.General.ValueIsInvalid(nameof(petStatus));
        }

        return Create(petStatusValue);
    }

    public static PetStatus Create(PetStatusValue petStatus)
    {
        return new PetStatus(petStatus);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Status;
    }
}
