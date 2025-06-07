using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Common;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Specieses.Application.Commands.DeleteSpeciesById;

[UsedImplicitly]
public sealed class DeleteSpeciesByIdCommandHandler : ICommandHandler<DeleteSpeciesByIdCommand>
{
    private readonly ISpeciesReadDbContext _readDbContext;
    private readonly ISpeciesRepository _repository;
    private readonly TimeProvider _timeProvider;
    private readonly IVolunteersContract _volunteersContract;

    public DeleteSpeciesByIdCommandHandler(ISpeciesRepository repository, ISpeciesReadDbContext readDbContext, TimeProvider timeProvider,
        IVolunteersContract volunteersContract)
    {
        _repository = repository;
        _readDbContext = readDbContext;
        _timeProvider = timeProvider;
        _volunteersContract = volunteersContract;
    }

    public async Task<UnitResult<ErrorList>> Handle(DeleteSpeciesByIdCommand command, CancellationToken cancellationToken)
    {
        var species = await _repository.GetById(command.SpeciesId, cancellationToken);

        if (species.IsFailure)
        {
            return species.Error.ToErrorList();
        }

        var existsPet = await _volunteersContract.CheckPetExistsBySpeciesId(new CheckPetExistsBySpeciesIdRequest(command.SpeciesId), cancellationToken);

        if (existsPet.IsFailure)
        {
            return Error.Conflict("species.delete", $"Could not delete species '{command.SpeciesId}' because exists pet with this species").ToErrorList();
        }

        species.Value.SoftDelete(_timeProvider.GetUtcNow().UtcDateTime);
        await _repository.Save(species.Value, cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}

public record DeleteSpeciesByIdCommand(Guid SpeciesId) : ICommand;
