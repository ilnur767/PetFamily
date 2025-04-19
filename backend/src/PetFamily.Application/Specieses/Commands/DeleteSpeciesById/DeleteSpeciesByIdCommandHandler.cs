using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Specieses.Commands.DeleteSpeciesById;

[UsedImplicitly]
public sealed class DeleteSpeciesByIdCommandHandler : ICommandHandler<DeleteSpeciesByIdCommand>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ISpeciesRepository _repository;
    private readonly TimeProvider _timeProvider;

    public DeleteSpeciesByIdCommandHandler(ISpeciesRepository repository, IReadDbContext readDbContext, TimeProvider timeProvider)
    {
        _repository = repository;
        _readDbContext = readDbContext;
        _timeProvider = timeProvider;
    }

    public async Task<UnitResult<ErrorList>> Handle(DeleteSpeciesByIdCommand command, CancellationToken cancellationToken)
    {
        var species = await _repository.GetById(command.SpeciesId, cancellationToken);

        if (species.IsFailure)
        {
            return species.Error.ToErrorList();
        }

        var existsPet = await _readDbContext.Pets.AnyAsync(p => p.SpeciesId == command.SpeciesId && p.IsDeleted == false, cancellationToken);

        if (existsPet)
        {
            return Error.Conflict("species.delete", $"Could not delete species '{command.SpeciesId}' because exists pet with this species").ToErrorList();
        }

        species.Value.SoftDelete(_timeProvider.GetUtcNow().UtcDateTime);
        await _repository.Save(species.Value, cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}

public record DeleteSpeciesByIdCommand(Guid SpeciesId) : ICommand;
