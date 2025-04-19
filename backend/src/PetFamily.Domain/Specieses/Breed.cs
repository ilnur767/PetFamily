using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;
using static PetFamily.Domain.Common.ValidationMessageConstants;

namespace PetFamily.Domain.Specieses;

public class Breed : SoftDeletableEntity<BreedId>
{
    private Breed(BreedId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public static Result<Breed, Error> Create(BreedId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Errors.General.ValueIsInvalid(string.Format(EmptyPropertyTemplate, "Breed name"));
        }

        return new Breed(id, name);
    }
}
