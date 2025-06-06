using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Common;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Specieses.Application.Commands.DeleteBreedById;

[UsedImplicitly]
public class DeleteBreedByIdCommandHandler : ICommandHandler<DeleteBreedByIdCommand>
{
    private readonly ISpeciesReadDbContext _readDbContext;
    private readonly ISpeciesRepository _repository;
    private readonly TimeProvider _timeProvider;
    private readonly IVolunteersContract _volunteersContract;

    public DeleteBreedByIdCommandHandler(ISpeciesReadDbContext readDbContext, ISpeciesRepository repository, TimeProvider timeProvider,
        IVolunteersContract volunteersContract)
    {
        _readDbContext = readDbContext;
        _repository = repository;
        _timeProvider = timeProvider;
        _volunteersContract = volunteersContract;
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

        var existsPet = await _volunteersContract.CheckPetExistsBySpeciesIdAndBreedId(
            new CheckPetExistsBySpeciesAndBreedIdRequest(command.SpeciesId, command.BreedId), cancellationToken);

        if (existsPet.IsFailure)
        {
            return Error.Conflict("breed.delete", $"Could not delete breed '{command.BreedId}' because exists pet with this breed").ToErrorList();
        }

        breed.SoftDelete(_timeProvider.GetUtcNow().UtcDateTime);
        await _repository.Save(species.Value, cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}

public record DeleteBreedByIdCommand(Guid SpeciesId, Guid BreedId) : ICommand;
