using System.Text;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Common;
using static PetFamily.SharedKernel.Common.ValidationMessageConstants;
using static PetFamily.SharedKernel.Common.Errors;

namespace PetFamily.SharedKernel.ValueObjects;

public class Requisite : ComparableValueObject
{
    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }

    public static Result<Requisite, Error> Create(string name, string description)
    {
        var errorMessage = new StringBuilder();

        if (string.IsNullOrWhiteSpace(name))
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, "Requisite name"));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, "Requisite description"));
        }

        if (errorMessage.Length > 0)
        {
            return Error.Validation(InvalidValueCode, errorMessage.ToString());
        }

        return new Requisite(name, description);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Name;
        yield return Description;
    }
}
