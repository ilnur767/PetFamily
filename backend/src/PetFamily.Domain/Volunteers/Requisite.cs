using System.Text;
using CSharpFunctionalExtensions;
using static PetFamily.Domain.Common.ValidationMessageConstants;

namespace PetFamily.Domain.Volunteers;

public class Requisite : ComparableValueObject
{
    public string Name { get; }
    public string Description { get; }

    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Result<Requisite> Create(string name, string description)
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
            return Result.Failure<Requisite>(errorMessage.ToString());
        }

        return Result.Success(new Requisite(name, description));
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Name;
        yield return Description;
    }
}