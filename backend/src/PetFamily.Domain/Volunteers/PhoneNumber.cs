using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using static PetFamily.Domain.Common.ValidationMessageConstants;

namespace PetFamily.Domain.Volunteers;

public class PhoneNumber : ComparableValueObject
{
    private const string PhoneNumberMatchPattern =
        @"^\+?\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$";

    private PhoneNumber(string phoneNumber)
    {
        Value = phoneNumber;
    }

    public string Value { get; }

    public static Result<PhoneNumber> Create(string phoneNumber)
    {
        if (!Regex.IsMatch(phoneNumber, PhoneNumberMatchPattern))
        {
            Result.Failure<PhoneNumber>(string.Format(InvalidPropertyTemplate, nameof(PhoneNumber)));
        }

        return Result.Success(new PhoneNumber(phoneNumber));
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}