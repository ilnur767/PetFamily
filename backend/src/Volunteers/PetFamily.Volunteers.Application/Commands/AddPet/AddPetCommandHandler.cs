using CSharpFunctionalExtensions;
using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Common;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Specieses.Contracts;
using PetFamily.Specieses.Contracts.Requests;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.AddPet;

[UsedImplicitly]
public sealed class AddPetCommandHandler : ICommandHandler<Guid, AddPetCommand>
{
    private readonly IVolunteerReadDbContext _readDbContext;
    private readonly ISpeciesContract _speciesContract;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public AddPetCommandHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<AddPetCommand> validator,
        IVolunteerReadDbContext readDbContext,
        ISpeciesContract speciesContract)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _readDbContext = readDbContext;
        _speciesContract = speciesContract;
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
            await _speciesContract.CheckBreedToSpeciesExistsHandler(new CheckBreedToSpeciesExistsRequest(command.SpeciesId, command.BreedId),
                cancellationToken);

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
}

public record AddPetCommand(
    Guid VolunteerId,
    string NickName,
    string Description,
    string PhoneNumber,
    string PetStatus,
    Guid SpeciesId,
    Guid BreedId) : ICommand;
