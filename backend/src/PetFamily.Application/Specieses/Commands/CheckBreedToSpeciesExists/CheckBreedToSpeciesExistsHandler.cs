using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Specieses.Commands.CheckBreedToSpeciesExists;

public sealed class CheckBreedToSpeciesExistsHandler : ICommandHandler<CheckBreedToSpeciesExistsCommand>
{
    private readonly IReadDbContext _readDbContext;

    public CheckBreedToSpeciesExistsHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<UnitResult<ErrorList>> Handle(CheckBreedToSpeciesExistsCommand command, CancellationToken cancellationToken)
    {
        var speices = await _readDbContext.Specieses.AnyAsync(s => s.Id == command.SpeciesId, cancellationToken);

        if (speices == false)
        {
            return Errors.General.NotFound(command.SpeciesId).ToErrorList();
        }

        var breed = await _readDbContext.Breeds
            .AnyAsync(b => b.Id == command.BreedId && b.SpeciesId == command.SpeciesId, cancellationToken);

        if (breed == false)
        {
            return Errors.General.NotFound(command.BreedId).ToErrorList();
        }

        return new UnitResult<ErrorList>();
    }
}

public record CheckBreedToSpeciesExistsCommand(Guid SpeciesId, Guid BreedId) : ICommand;
