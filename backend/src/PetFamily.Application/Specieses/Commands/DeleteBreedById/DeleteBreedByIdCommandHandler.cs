using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Specieses.Commands.DeleteBreedById;

[UsedImplicitly]
public class DeleteBreedByIdCommandHandler : ICommandHandler<DeleteBreedByIdCommand>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ISpeciesRepository _repository;
    private readonly TimeProvider _timeProvider;

    public DeleteBreedByIdCommandHandler(IReadDbContext readDbContext, ISpeciesRepository repository, TimeProvider timeProvider)
    {
        _readDbContext = readDbContext;
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public async Task<UnitResult<ErrorList>> Handle(DeleteBreedByIdCommand command, CancellationToken cancellationToken)
    {
        var species = await _repository.GetById(command.SpeciesId, cancellationToken);

        if (species.IsFailure)
        {
            return species.Error.ToErrorList();
        }

        var breed = species.Value.Breeds.FirstOrDefault(b => b.Id.Value == command.BreedId);

        if (breed == null)
        {
            return Error.NotFound("breed", $"Could not found breed '{command.BreedId}'").ToErrorList();
        }

        var existsPet = await _readDbContext.Pets
            .AnyAsync(p =>
                    p.SpeciesId == command.SpeciesId &&
                    p.BreedId == command.BreedId &&
                    p.IsDeleted == false,
                cancellationToken);

        if (existsPet)
        {
            return Error.Conflict("breed.delete", $"Could not delete breed '{command.BreedId}' because exists pet with this breed").ToErrorList();
        }

        breed.SoftDelete(_timeProvider.GetUtcNow().UtcDateTime);
        await _repository.Save(species.Value, cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}

public record DeleteBreedByIdCommand(Guid SpeciesId, Guid BreedId) : ICommand
{
}
