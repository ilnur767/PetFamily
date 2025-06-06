using System.Net.Mail;
using AutoFixture;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Specieses.Domain.Specieses;
using PetFamily.Volunteers.Application.Commands.AddPet;
using PetFamily.Volunteers.Application.Commands.Create;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.IntegrationTests;

public static class FixtureExtensions
{
    public static CreateVolunteerCommand CreateVolunteerCommand(this Fixture fixture)
    {
        return fixture.Build<CreateVolunteerCommand>()
            .With(c => c.Email, fixture.Create<MailAddress>().Address)
            .With(c => c.PhoneNumber, "89992343432")
            .Create();
    }

    public static AddPetCommand CreatePetCommand(this Fixture fixture, Guid volunteerId, Guid speciesId, Guid breedId)
    {
        return fixture.Build<AddPetCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.SpeciesId, speciesId)
            .With(c => c.BreedId, breedId)
            .With(c => c.PetStatus, fixture.Create<PetStatusValue>().ToString())
            .With(c => c.PhoneNumber, "89992343432")
            .Create();
    }

    public static Volunteer CreateVolunteer(this Fixture fixture)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        var fullName = FullName.Create(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>()).Value;
        var email = Email.Create(fixture.Create<MailAddress>().Address).Value;
        var phoneNumber = PhoneNumber.Create("89992343432").Value;

        var volunteer = new Volunteer(volunteerId, fullName, email, phoneNumber);

        return volunteer;
    }

    public static Species CreateSpecies(this Fixture fixture)
    {
        var speciesId = SpeciesId.NewSpeciesId();

        var species = Species.Create(speciesId, fixture.Create<string>()).Value;

        return species;
    }

    public static Breed CreateBreed(this Fixture fixture)
    {
        var breedId = BreedId.NewBreedId();

        var breed = Breed.Create(breedId, fixture.Create<string>()).Value;

        return breed;
    }

    public static Pet CreatePet(this Fixture fixture, Guid? speciesId = null, Guid? breedId = null)
    {
        var petId = PetId.NewPetId();
        var petStatus = PetStatus.Create(fixture.Create<PetStatusValue>());

        var phoneNumber = PhoneNumber.Create("89992343432").Value;

        var petSpesies = PetSpecies.Create(SpeciesId.Create(speciesId ?? Guid.NewGuid()), BreedId.Create(breedId ?? Guid.NewGuid())).Value;

        var pet = Pet.Create(petId, fixture.Create<string>(), fixture.Create<string>(), phoneNumber, petSpesies, petStatus).Value;

        return pet;
    }
}
