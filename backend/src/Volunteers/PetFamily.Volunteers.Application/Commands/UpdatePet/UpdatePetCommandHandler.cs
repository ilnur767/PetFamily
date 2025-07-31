using CSharpFunctionalExtensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Common;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Specieses.Contracts;
using PetFamily.Specieses.Contracts.Requests;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.UpdatePet;

[UsedImplicitly]
public sealed class UpdatePetCommandHandler : ICommandHandler<Guid, UpdatePetCommand>
{
    private readonly ILogger<UpdatePetCommandHandler> _logger;
    private readonly ISpeciesContract _speciesContract;
    private readonly IValidator<UpdatePetCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public UpdatePetCommandHandler(
        IValidator<UpdatePetCommand> validator,
        IVolunteersRepository volunteersRepository,
        ILogger<UpdatePetCommandHandler> logger,
        ISpeciesContract speciesContract)
    {
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _speciesContract = speciesContract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdatePetCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorList();
        }

        var volunteer = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return volunteer.Error.ToErrorList();
        }

        // получить по контракту
        var existsResult =
            await _speciesContract.CheckBreedToSpeciesExistsHandler(new CheckBreedToSpeciesExistsRequest(command.SpeciesId, command.BreedId),
                cancellationToken);

        if (existsResult.IsFailure)
        {
            return existsResult.Error;
        }

        var petSpecies = new PetSpecies(SpeciesId.Create(command.SpeciesId), BreedId.Create(command.BreedId));

        var updatePetResult = volunteer.Value.UpdatePet(
            command.PetId,
            petSpecies,
            command.NickName,
            command.Description,
            PhoneNumber.Create(command.PhoneNumber).Value,
            command.Height,
            command.Weight,
            command.IsCastrated,
            command.IsVaccinated,
            command.DateOfBirth,
            command.HealthInformation,
            command.Address);

        if (updatePetResult.IsFailure)
        {
            return updatePetResult.Error.ToErrorList();
        }

        await _volunteersRepository.Save(volunteer.Value, cancellationToken);

        _logger.LogInformation("Updated main info for pet with id: {id}", command.PetId);

        return command.PetId;
    }
}

public record UpdatePetCommand(
    Guid VolunteerId,
    Guid PetId,
    string NickName,
    string Description,
    string PhoneNumber,
    double Height,
    double Weight,
    bool IsCastrated,
    bool IsVaccinated,
    DateTime DateOfBirth,
    string Address,
    string HealthInformation,
    Guid SpeciesId,
    Guid BreedId) : ICommand;
