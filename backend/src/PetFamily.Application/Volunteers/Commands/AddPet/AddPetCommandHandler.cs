using CSharpFunctionalExtensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Specieses;
using PetFamily.Application.Specieses.Commands.CheckBreedToSpeciesExists;
using PetFamily.Domain.Common;
using PetFamily.Domain.Specieses;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.AddPet;

[UsedImplicitly]
public sealed class AddPetCommandHandler : ICommandHandler<Guid, AddPetCommand>
{
    private readonly ICommandHandler<CheckBreedToSpeciesExistsCommand> _checkBreedToSpeciesExistsHandler;
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public AddPetCommandHandler(
        IVolunteersRepository volunteersRepository,
        ISpeciesRepository speciesRepository,
        IValidator<AddPetCommand> validator,
        IReadDbContext readDbContext,
        ICommandHandler<CheckBreedToSpeciesExistsCommand> checkBreedToSpeciesExistsHandler)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _readDbContext = readDbContext;
        _checkBreedToSpeciesExistsHandler = checkBreedToSpeciesExistsHandler;
    }

    public async Task<Result<Guid, ErrorList>> Handle(AddPetCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteer = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return volunteer.Error.ToErrorList();
        }

        var existsResult =
            await _checkBreedToSpeciesExistsHandler.Handle(new CheckBreedToSpeciesExistsCommand(command.SpeciesId, command.BreedId), cancellationToken);
        if (existsResult.IsFailure)
        {
            return existsResult.Error;
        }

        var petSpecies = new PetSpecies(SpeciesId.Create(command.SpeciesId), BreedId.Create(command.BreedId));

        var petStatus = PetStatus.Create(command.PetStatus).Value;

        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var petId = PetId.NewPetId();
        var pet = Pet.Create(petId, command.NickName, command.Description, phoneNumber, petSpecies, petStatus);

        var result = volunteer.Value.AddPet(pet.Value);

        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        await _volunteersRepository.Save(volunteer.Value, cancellationToken);

        return petId.Value;
    }

    private async Task<UnitResult<Error>> CheckBreedToSpeciesExists(AddPetCommand command, CancellationToken cancellationToken)
    {
        var speices = await _readDbContext.Specieses.AnyAsync(s => s.Id == command.SpeciesId, cancellationToken);

        if (speices == false)
        {
            return Errors.General.NotFound(command.SpeciesId);
        }

        var breed = await _readDbContext.Breeds
            .AnyAsync(b => b.Id == command.BreedId && b.SpeciesId == command.SpeciesId, cancellationToken);

        if (breed == false)
        {
            return Errors.General.NotFound(command.BreedId);
        }

        return new UnitResult<Error>();
    }
}

public record AddPetCommand(
    Guid VolunteerId,
    string NickName,
    string Description,
    string PhoneNumber,
    string PetStatus,
    Guid SpeciesId,
    Guid BreedId) : ICommand;
