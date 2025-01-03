using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using static PetFamily.Domain.Common.ValidationMessageConstants;

namespace PetFamily.Domain.Volunteers;

public class Email : ComparableValueObject
{
    private const string EmailMatchPattern =
        @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

    private Email(string emailAddress)
    {
        Address = emailAddress;
    }

    public string Address { get; }

    public static Result<Email> Create(string email)
    {
        if (!Regex.IsMatch(email, EmailMatchPattern))
        {
            return Result.Failure<Email>(string.Format(InvalidPropertyTemplate, nameof(PhoneNumber)));
        }

        return Result.Success(new Email(email));
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Address;
    }
}