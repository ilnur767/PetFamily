using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Common;
using PetFamily.Volunteers.Application.Commands.CheckPetExistsBySpeciesAndBreedId;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Volunteers.Presentation;

public class VolunteersContract : IVolunteersContract
{
    private readonly ICommandHandler<CheckPetExistsBySpeciesIdAndBreedIdCommand> _checkPetExistsBySpeciesIdAndBreedIdCommand;
    private readonly ICommandHandler<CheckPetExistsBySpeciesIdCommand> _checkPetExistsBySpeciesIdCommand;

    public VolunteersContract(
        ICommandHandler<CheckPetExistsBySpeciesIdCommand> checkPetExistsBySpeciesIdCommand,
        ICommandHandler<CheckPetExistsBySpeciesIdAndBreedIdCommand> checkPetExistsBySpeciesIdAndBreedIdCommand)
    {
        _checkPetExistsBySpeciesIdCommand = checkPetExistsBySpeciesIdCommand;
        _checkPetExistsBySpeciesIdAndBreedIdCommand = checkPetExistsBySpeciesIdAndBreedIdCommand;
    }

    public async Task<UnitResult<ErrorList>> CheckPetExistsBySpeciesIdAndBreedId(CheckPetExistsBySpeciesAndBreedIdRequest request,
        CancellationToken cancellationToken)
    {
        return await _checkPetExistsBySpeciesIdAndBreedIdCommand
            .Handle(new CheckPetExistsBySpeciesIdAndBreedIdCommand(request.SpesiesId, request.BreedId), cancellationToken);
    }

    public async Task<UnitResult<ErrorList>> CheckPetExistsBySpeciesId(CheckPetExistsBySpeciesIdRequest request, CancellationToken cancellationToken)
    {
        return await _checkPetExistsBySpeciesIdCommand.Handle(new CheckPetExistsBySpeciesIdCommand(request.SpesiesId), cancellationToken);
    }
}
