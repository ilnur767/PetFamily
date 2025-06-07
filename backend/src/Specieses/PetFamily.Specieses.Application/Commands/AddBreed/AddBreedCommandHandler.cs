using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Common;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Specieses.Domain.Specieses;

namespace PetFamily.Specieses.Application.Commands.AddBreed;

public class AddBreedCommandHandler : ICommandHandler<Guid, CreateBreedCommand>
{
    private readonly ISpeciesRepository _speciesRepository;

    public AddBreedCommandHandler(ISpeciesRepository speciesRepository)
    {
        _speciesRepository = speciesRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateBreedCommand command, CancellationToken cancellationToken)
    {
        var species = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);

        if (species.IsFailure)
        {
            return species.Error.ToErrorList();
        }

        var breed = Breed.Create(BreedId.NewBreedId(), command.Name);

        if (breed.IsFailure)
        {
            return breed.Error.ToErrorList();
        }

        species.Value.AddBreed(breed.Value);

        await _speciesRepository.Save(species.Value, cancellationToken);

        return breed.Value.Id.Value;
    }
}

public record CreateBreedCommand(Guid SpeciesId, string Name) : ICommand;
