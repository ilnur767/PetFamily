using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;
using static PetFamily.Domain.Common.ValidationMessageConstants;

namespace PetFamily.Domain.Volunteers;

public class PhoneNumber : ComparableValueObject
{
    private const string PhoneNumberMatchPattern =
        @"^\+?\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$";

    // ef
    private PhoneNumber() { }
    
    private PhoneNumber(string phoneNumber)
    {
        Value = phoneNumber;
    }

    public string Value { get; }

    public static Result<PhoneNumber, Error> Create(string phoneNumber)
    {
        if (!Regex.IsMatch(phoneNumber, PhoneNumberMatchPattern))
        {
            return Errors.General.ValueIsInvalid(nameof(PhoneNumber));
        }

        return new PhoneNumber(phoneNumber);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}