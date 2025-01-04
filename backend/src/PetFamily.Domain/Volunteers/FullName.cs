using System.Text;
using CSharpFunctionalExtensions;
using static PetFamily.Domain.Common.ValidationMessageConstants;

namespace PetFamily.Domain.Volunteers;

public class FullName : ComparableValueObject
{
    public FullName(string firstName, string lastName, string middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public string MiddleName { get; }

    public static Result<FullName> Create(string firstName, string lastName, string middleName)
    {
        var errorMessage = new StringBuilder();

        if (string.IsNullOrWhiteSpace(firstName))
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, nameof(FirstName)));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, nameof(LastName)));
        }

        if (string.IsNullOrWhiteSpace(middleName))
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, nameof(MiddleName)));
        }

        if (errorMessage.Length > 0)
        {
            return Result.Failure<FullName>(errorMessage.ToString());
        }

        return Result.Success(new FullName(firstName, lastName, middleName));
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return MiddleName;
    }
}