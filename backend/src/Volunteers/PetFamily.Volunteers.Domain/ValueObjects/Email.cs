using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public class Email : ComparableValueObject
{
    private const string EmailMatchPattern =
        @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

    // ef
    private Email()
    {
    }

    private Email(string emailAddress)
    {
        Address = emailAddress;
    }

    public string Address { get; }

    public static Result<Email, Error> Create(string email)
    {
        if (!Regex.IsMatch(email, EmailMatchPattern))
        {
            return Errors.General.ValueIsInvalid(nameof(Email));
        }

        return new Email(email);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Address;
    }
}
