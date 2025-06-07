using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Common;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Specieses.Domain.Specieses;

namespace PetFamily.Specieses.Application.Commands.AddSpecies;

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
