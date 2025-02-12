using System.Text;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;
using static PetFamily.Domain.Common.ValidationMessageConstants;
using static PetFamily.Domain.Common.Errors;

namespace PetFamily.Domain.Volunteers;

public class FullName : ComparableValueObject
{
    // ef
    private FullName()
    {
    }

    private FullName(string firstName, string lastName, string middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public string MiddleName { get; }

    public static Result<FullName, Error> Create(string firstName, string lastName, string middleName)
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
            return Error.Validation(InvalidValueCode, errorMessage.ToString());
        }

        return new FullName(firstName, lastName, middleName);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return MiddleName;
    }
}