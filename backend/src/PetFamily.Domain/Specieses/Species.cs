using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;
using static PetFamily.Domain.Common.ValidationMessageConstants;
using static PetFamily.Domain.Common.Errors;

namespace PetFamily.Domain.Specieses;

public class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = new();

    public Species(SpeciesId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public IReadOnlyList<Breed> Breeds => _breeds;

    public Result<Species> Create(SpeciesId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Species>(string.Format(EmptyPropertyTemplate, "Species name"));
        }

        return Result.Success(new Species(id, name));
    }

    public Result<Breed, Error> GetBreedById(Guid id)
    {
        var breed = Breeds.FirstOrDefault(b => b.Id.Value.Equals(id));
        if (breed == null)
        {
            return Error.NotFound(RecordNotFoundCode, $"Breed not found for {id}");
        }

        return breed;
    }
}
