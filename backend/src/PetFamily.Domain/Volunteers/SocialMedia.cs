using System.Text;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;
using static PetFamily.Domain.Common.ValidationMessageConstants;
using static PetFamily.Domain.Common.Errors;

namespace PetFamily.Domain.Volunteers;

public class SocialMedia : ComparableValueObject
{
    private SocialMedia(string name, string link)
    {
        Name = name;
        Link = link;
    }

    public string Name { get; }
    public string Link { get; }

    public static Result<SocialMedia, Error> Create(string name, string link)
    {
        var errorMessage = new StringBuilder();

        if (string.IsNullOrWhiteSpace(name))
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, "SocialMedia name"));
        }

        if (string.IsNullOrWhiteSpace(link))
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, "SocialMedia link"));
        }

        if (errorMessage.Length > 0)
        {
            return Error.Validation(InvalidValueCode, errorMessage.ToString());
        }

        return new SocialMedia(name, link);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Link;
        yield return Name;
    }
}