using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using PetFamily.SharedKernel.Common;
using PetFamily.Specieses.Application.Commands.AddBreed;
using PetFamily.Specieses.Application.Commands.AddSpecies;
using PetFamily.Specieses.Application.Commands.CheckBreedToSpeciesExists;
using PetFamily.Specieses.Application.Commands.DeleteBreedById;
using PetFamily.Specieses.Application.Commands.DeleteSpeciesById;
using PetFamily.Specieses.Application.Queries;
using PetFamily.Specieses.Contracts;
using PetFamily.Specieses.Contracts.Requests;

namespace PetFamily.Specieses.Presentation;

public class SpeciesContract : ISpeciesContract
{
    private readonly ICommandHandler<CheckBreedToSpeciesExistsCommand> _checkBreedToSpeciesExistsCommand;
    private readonly ICommandHandler<Guid, CreateBreedCommand> _createBreedCommand;
    private readonly ICommandHandler<Guid, CreateSpeciesCommand> _createSpeciesCommand;
    private readonly ICommandHandler<DeleteBreedByIdCommand> _deleteBreedByIdCommand;
    private readonly ICommandHandler<DeleteSpeciesByIdCommand> _deleteSpeciesByIdCommand;
    private readonly IQueryHandler<PagedList<BreedDto>, GetBreedsBySpeiesIdWithPaginationQuery> _getBreedsBySpeiesIdWithPaginationQuery;

    private readonly IQueryHandler<PagedList<SpeciesDto>, GetSpeiciesWithPaginationQuery> _getSpesiesdWithPaginationQuery;

    public SpeciesContract(ICommandHandler<Guid, CreateSpeciesCommand> createSpeciesCommand, ICommandHandler<Guid, CreateBreedCommand> createBreedCommand,
        ICommandHandler<DeleteSpeciesByIdCommand> deleteSpeciesByIdCommand, ICommandHandler<DeleteBreedByIdCommand> deleteBreedByIdCommand,
        IQueryHandler<PagedList<BreedDto>, GetBreedsBySpeiesIdWithPaginationQuery> getBreedsBySpeiesIdWithPaginationQuery,
        IQueryHandler<PagedList<SpeciesDto>, GetSpeiciesWithPaginationQuery> getSpesiesdWithPaginationQuery,
        ICommandHandler<CheckBreedToSpeciesExistsCommand> checkBreedToSpeciesExistsCommand)
    {
        _createSpeciesCommand = createSpeciesCommand;
        _createBreedCommand = createBreedCommand;
        _deleteSpeciesByIdCommand = deleteSpeciesByIdCommand;
        _deleteBreedByIdCommand = deleteBreedByIdCommand;
        _getBreedsBySpeiesIdWithPaginationQuery = getBreedsBySpeiesIdWithPaginationQuery;
        _getSpesiesdWithPaginationQuery = getSpesiesdWithPaginationQuery;
        _checkBreedToSpeciesExistsCommand = checkBreedToSpeciesExistsCommand;
    }

    public async Task<Result<Guid, ErrorList>> CreateSpecies(CreateSpeciesRequest request, CancellationToken cancellationToken)
    {
        return await _createSpeciesCommand.Handle(new CreateSpeciesCommand(request.Name), cancellationToken);
    }

    public async Task<Result<Guid, ErrorList>> CreateBreed(Guid speciesId, CreateBreedRequest request, CancellationToken cancellationToken)
    {
        return await _createBreedCommand.Handle(new CreateBreedCommand(speciesId, request.Name), cancellationToken);
    }

    public async Task<UnitResult<ErrorList>> DeleteSpecies(Guid spesiesId, CancellationToken cancellationToken)
    {
        return await _deleteSpeciesByIdCommand.Handle(new DeleteSpeciesByIdCommand(spesiesId), cancellationToken);
    }

    public async Task<UnitResult<ErrorList>> DeleteBreed(Guid speciesId, Guid breedId, CancellationToken cancellationToken)
    {
        return await _deleteBreedByIdCommand.Handle(new DeleteBreedByIdCommand(speciesId, breedId), cancellationToken);
    }

    public async Task<Result<PagedList<SpeciesDto>?, ErrorList>> GetSpeiciesWithPaginationQuery(GetSpeciesesWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        return await _getSpesiesdWithPaginationQuery.Handle(new GetSpeiciesWithPaginationQuery(request.Page, request.PageSize), cancellationToken);
    }

    public async Task<Result<PagedList<BreedDto>?, ErrorList>> GetBreedsBySpeciesIdWithPagination(Guid spesiesId,
        GetBreedsBySpeсiesIdWithPaginationRequest request, CancellationToken cancellationToken)
    {
        return await _getBreedsBySpeiesIdWithPaginationQuery.Handle(new GetBreedsBySpeiesIdWithPaginationQuery(spesiesId, request.Page, request.PageSize),
            cancellationToken);
    }

    public Task<UnitResult<ErrorList>> CheckBreedToSpeciesExistsHandler(CheckBreedToSpeciesExistsRequest reuRequest, CancellationToken cancellationToken)
    {
        return _checkBreedToSpeciesExistsCommand.Handle(new CheckBreedToSpeciesExistsCommand(reuRequest.SpeciesId, reuRequest.BreedId), cancellationToken);
    }
}
