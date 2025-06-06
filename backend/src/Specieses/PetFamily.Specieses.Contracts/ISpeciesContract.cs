using CSharpFunctionalExtensions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using PetFamily.SharedKernel.Common;
using PetFamily.Specieses.Contracts.Requests;

namespace PetFamily.Specieses.Contracts;

public interface ISpeciesContract
{
    Task<Result<Guid, ErrorList>> CreateSpecies(CreateSpeciesRequest request, CancellationToken cancellationToken);
    Task<Result<Guid, ErrorList>> CreateBreed(Guid speciesId, CreateBreedRequest request, CancellationToken cancellationToken);
    Task<UnitResult<ErrorList>> DeleteSpecies(Guid spesiesId, CancellationToken cancellationToken);
    Task<UnitResult<ErrorList>> DeleteBreed(Guid speciesId, Guid breedId, CancellationToken cancellationToken);

    Task<Result<PagedList<SpeciesDto>?, ErrorList>> GetSpeiciesWithPaginationQuery(GetSpeciesesWithPaginationRequest request,
        CancellationToken cancellationToken);

    Task<Result<PagedList<BreedDto>?, ErrorList>> GetBreedsBySpeciesIdWithPagination(Guid spesiesId, GetBreedsBySpeсiesIdWithPaginationRequest request,
        CancellationToken cancellationToken);

    Task<UnitResult<ErrorList>> CheckBreedToSpeciesExistsHandler(CheckBreedToSpeciesExistsRequest reuRequest, CancellationToken cancellationToken);
}
