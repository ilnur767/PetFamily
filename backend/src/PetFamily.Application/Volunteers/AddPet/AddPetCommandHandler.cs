using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Application.Species;
using PetFamily.Domain.Common;
using PetFamily.Domain.Specieses;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.AddPet;

public sealed class AddPetCommandHandler
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<AddPetCommand> validator;

    public AddPetCommandHandler(IVolunteersRepository volunteersRepository, ISpeciesRepository speciesRepository, IValidator<AddPetCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
        this.validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(AddPetCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteer = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return volunteer.Error.ToErrorList();
        }

        var speices = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);

        if (speices.IsFailure)
        {
            return speices.Error.ToErrorList();
        }

        var breed = speices.Value.GetBreedById(command.BreedId);

        if (breed.IsFailure)
        {
            return breed.Error.ToErrorList();
        }

        var petSpecies = new PetSpecies(SpeciesId.Create(speices.Value.Id), BreedId.Create(command.BreedId));

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
    Guid BreedId);
