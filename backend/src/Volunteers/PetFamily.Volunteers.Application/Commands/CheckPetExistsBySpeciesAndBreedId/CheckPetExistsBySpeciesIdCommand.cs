using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Volunteers.Application.Commands.CheckPetExistsBySpeciesAndBreedId;

public sealed class CheckPetExistsBySpeciesIdHandler : ICommandHandler<CheckPetExistsBySpeciesIdCommand>
{
    private readonly IVolunteerReadDbContext _volunteerReadDbContext;

    public CheckPetExistsBySpeciesIdHandler(IVolunteerReadDbContext volunteerReadDbContext)
    {
        _volunteerReadDbContext = volunteerReadDbContext;
    }

    public async Task<UnitResult<ErrorList>> Handle(CheckPetExistsBySpeciesIdCommand command, CancellationToken cancellationToken)
    {
        var exists = await _volunteerReadDbContext.Pets.AnyAsync(p =>
                p.SpeciesId == command.SpeciesId &&
                p.IsDeleted == false,
            cancellationToken);

        if (!exists)
        {
            return Errors.General.NotFound($"no pet with speciesId '{command.SpeciesId}' was found").ToErrorList();
        }

        return Result.Success<ErrorList>();
    }
}

public record CheckPetExistsBySpeciesIdCommand(Guid SpeciesId) : ICommand;
