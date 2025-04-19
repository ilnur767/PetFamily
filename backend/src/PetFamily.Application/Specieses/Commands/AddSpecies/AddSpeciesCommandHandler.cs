using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Common;
using PetFamily.Domain.Specieses;

namespace PetFamily.Application.Specieses.Commands.AddSpecies;

[UsedImplicitly]
public sealed class CreateSpeciesCommandHandler : ICommandHandler<Guid, CreateSpeciesCommand>
{
    private readonly ISpeciesRepository _repository;

    public CreateSpeciesCommandHandler(ISpeciesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateSpeciesCommand command, CancellationToken cancellationToken)
    {
        var species = Species.Create(SpeciesId.NewSpeciesId(), command.Name).Value;

        var result = await _repository.Add(species, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        return result.Value;
    }
}

public record CreateSpeciesCommand(string Name) : ICommand;
