using System.Text;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.ValueObjects.Ids;
using static PetFamily.SharedKernel.Common.ValidationMessageConstants;

namespace PetFamily.Volunteers.Domain.ValueObjects;

public class PetSpecies : ComparableValueObject
{
    public PetSpecies(SpeciesId speciesId, BreedId breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public SpeciesId SpeciesId { get; }

    public BreedId BreedId { get; }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return SpeciesId;
        yield return BreedId;
    }

    public static Result<PetSpecies> Create(SpeciesId speciesId, BreedId breedId)
    {
        var errorMessage = new StringBuilder();

        if (speciesId is null)
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, nameof(SpeciesId)));
        }

        if (breedId is null)
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, nameof(BreedId)));
        }

        if (errorMessage.Length > 0)
        {
            return Result.Failure<PetSpecies>(errorMessage.ToString());
        }

        return Result.Success(new PetSpecies(speciesId, breedId));
    }
}
