using CSharpFunctionalExtensions;
using PetFamily.Application.Species;
using PetFamily.Domain.Common;
using PetFamily.Domain.Specieses;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.AddPet;

public sealed class AddPetCommandHandler
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IVolunteersRepository _volunteersRepository;

    public AddPetCommandHandler(IVolunteersRepository volunteersRepository, ISpeciesRepository speciesRepository)
    {
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
    }

    public async Task<Result<Guid, Error>> Handle(AddPetCommand command, CancellationToken cancellationToken)
    {
        var volunteer = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return volunteer.Error;
        }

        var speices = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);

        if (speices.IsFailure)
        {
            return speices.Error;
        }

        var breed = speices.Value.GetBreedById(command.BreedId);

        if (breed.IsFailure)
        {
            return breed.Error;
        }

        var petSpecies = new PetSpecies(SpeciesId.Create(speices.Value.Id), BreedId.Create(command.BreedId));

        var petStatus = PetStatus.Create(command.PetStatus).Value;

        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var petId = PetId.NewPetId();
        var pet = Pet.Create(petId, command.NickName, command.Description, phoneNumber, petSpecies, petStatus);

        var result = volunteer.Value.AddPet(pet.Value);

        if (result.IsFailure)
        {
            return result.Error;
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
