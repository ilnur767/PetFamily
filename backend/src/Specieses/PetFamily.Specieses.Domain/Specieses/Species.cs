using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Common;
using PetFamily.SharedKernel.ValueObjects.Ids;
using static PetFamily.SharedKernel.Common.DataLimitsConstants;
using static PetFamily.SharedKernel.Common.Errors;


namespace PetFamily.Specieses.Domain.Specieses;

public class Species : SoftDeletableEntity<SpeciesId>
{
    private readonly List<Breed> _breeds = new();

    public Species(SpeciesId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Result<Species, Error> Create(SpeciesId id, string speicesName)
    {
        if (string.IsNullOrWhiteSpace(speicesName) || speicesName.Length > MaxLowTextLength)
        {
            return General.ValueIsInvalid(nameof(speicesName));
        }

        return new Species(id, speicesName);
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

    public UnitResult<Error> AddBreed(Breed breed)
    {
        _breeds.Add(breed);

        return new UnitResult<Error>();
    }

    public override void SoftDelete(DateTime deletedAt)
    {
        DeletedAt = deletedAt;
        IsDeleted = true;

        foreach (var breed in _breeds)
        {
            breed.SoftDelete(deletedAt);
        }
    }
}
