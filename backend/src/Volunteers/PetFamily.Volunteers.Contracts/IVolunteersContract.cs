using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Common;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Volunteers.Contracts;

public interface IVolunteersContract
{
    Task<UnitResult<ErrorList>> CheckPetExistsBySpeciesIdAndBreedId(CheckPetExistsBySpeciesAndBreedIdRequest request, CancellationToken cancellationToken);
    Task<UnitResult<ErrorList>> CheckPetExistsBySpeciesId(CheckPetExistsBySpeciesIdRequest request, CancellationToken cancellationToken);
}
