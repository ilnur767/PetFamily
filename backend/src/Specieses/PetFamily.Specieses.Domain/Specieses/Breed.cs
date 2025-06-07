using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Common;
using PetFamily.SharedKernel.ValueObjects.Ids;
using static PetFamily.SharedKernel.Common.ValidationMessageConstants;

namespace PetFamily.Specieses.Domain.Specieses;

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
