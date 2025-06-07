using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Volunteers.Application.Commands.CheckPetExistsBySpeciesAndBreedId;

public sealed class CheckPetExistsBySpeciesIdAndBreedIdHandler : ICommandHandler<CheckPetExistsBySpeciesIdAndBreedIdCommand>
{
    private readonly IVolunteerReadDbContext _volunteerReadDbContext;

    public CheckPetExistsBySpeciesIdAndBreedIdHandler(IVolunteerReadDbContext volunteerReadDbContext)
    {
        _volunteerReadDbContext = volunteerReadDbContext;
    }

    public async Task<UnitResult<ErrorList>> Handle(CheckPetExistsBySpeciesIdAndBreedIdCommand command, CancellationToken cancellationToken)
    {
        var exists = await _volunteerReadDbContext.Pets.AnyAsync(p =>
                p.SpeciesId == command.SpeciesId &&
                p.BreedId == command.BreedId &&
                p.IsDeleted == false,
            cancellationToken);

        if (!exists)
        {
            return Errors.General.NotFound($"no pet with speciesId '{command.SpeciesId}' and breedId '{command.BreedId}' was found").ToErrorList();
        }

        return Result.Success<ErrorList>();
    }
}

public record CheckPetExistsBySpeciesIdAndBreedIdCommand(Guid SpeciesId, Guid BreedId) : ICommand;
