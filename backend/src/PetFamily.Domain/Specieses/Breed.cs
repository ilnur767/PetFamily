using CSharpFunctionalExtensions;
using static PetFamily.Domain.Common.ValidationMessageConstants;

namespace PetFamily.Domain.Specieses;

public class Breed : Entity<BreedId>
{
    public string Name { get; private set; }

    private Breed(BreedId id, string name) : base(id)
    {
        Name = name;
    }

    public static Result<Breed> Create(BreedId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Breed>(string.Format(EmptyPropertyTemplate, "Breed name"));
        }

        return Result.Success(new Breed(id, name));
    }
}