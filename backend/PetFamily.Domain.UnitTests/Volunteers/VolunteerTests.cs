using FluentAssertions;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects;
using Xunit;

namespace PetFamily.UnitTests.Volunteers;

public sealed class VolunteerTests
{
    [Fact]
    public void AddPet_WithFirstSerialNumber_Succeeds()
    {
        // Arrange
        var volunteerId = VolunteerId.NewVolunteerId();
        var fullName = FullName.Create("John Doe", "John Doe", "John Doe").Value;
        var email = Email.Create("john.doe@example.com").Value;
        var phone = PhoneNumber.Create("34987347377").Value;
        var volunteer = new Volunteer(volunteerId, fullName, email, phone);

        var petId = PetId.NewPetId();
        var species = PetSpecies.Create(SpeciesId.NewSpeciesId(), BreedId.NewBreedId()).Value;
        var petStatus = PetStatus.Create(nameof(PetStatusValue.NeedsHelp)).Value;
        var pet = Pet.Create(petId, "NickName", "Description", phone, species, petStatus).Value;

        // Act
        var result = volunteer.AddPet(pet);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.Position.Value.Should().Be(1);
    }

    [Fact]
    public void AddPet_WithOtherPets_Succeeds()
    {
        // Arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var petToAdd = Pet.Create(PetId.NewPetId(),
                "NickName",
                "Description",
                PhoneNumber.Create("3438483833").Value,
                PetSpecies.Create(SpeciesId.NewSpeciesId(), BreedId.NewBreedId()).Value,
                PetStatus.Create(PetStatusValue.NeedsHelp))
            .Value;

        // Act
        var result = volunteer.AddPet(petToAdd);

        // Assert
        var addedPet = volunteer.GetPetById(petToAdd.Id.Value);

        var serialNumber = Position.Create(petsCount + 1).Value;

        result.IsSuccess.Should().BeTrue();
        addedPet.IsSuccess.Should().BeTrue();
        addedPet.Value.Id.Should().Be(petToAdd.Id);
        addedPet.Value.Position.Should().Be(serialNumber);
    }

    private static Volunteer CreateVolunteerWithPets(int petsCount)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        var fullName = FullName.Create("John Doe", "John Doe", "John Doe").Value;
        var email = Email.Create("john.doe@example.com").Value;
        var phone = PhoneNumber.Create("34987347377").Value;
        var volunteer = new Volunteer(volunteerId, fullName, email, phone);

        var species = PetSpecies.Create(SpeciesId.NewSpeciesId(), BreedId.NewBreedId()).Value;
        var petStatus = PetStatus.Create(nameof(PetStatusValue.NeedsHelp)).Value;

        for (var i = 0; i < petsCount; i++)
        {
            var pet = Pet.Create(PetId.NewPetId(), "NickName", "Description", phone, species, petStatus).Value;
            volunteer.AddPet(pet);
        }

        return volunteer;
    }

    [Fact]
    public void MovePet_NewPositionEqualsCurrentPosition_Succeeds()
    {
        // Arrange
        var petsCount = 6;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // Act
        var result = volunteer.MovePet(secondPet, secondPosition);

        // Assert
        result.IsSuccess.Should().BeTrue();

        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(2);
        thirdPet.Position.Value.Should().Be(3);
        fourthPet.Position.Value.Should().Be(4);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void MovePet_MoveBackWhenNewPositionIsLower_Succeeds()
    {
        // Arrange
        var petsCount = 6;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // Act
        var result = volunteer.MovePet(fourthPet, secondPosition);

        // Assert
        result.IsSuccess.Should().BeTrue();

        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(2);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void MovePet_MoveForwardWhenNewPositionIsGreater_Succeeds()
    {
        // Arrange
        var petsCount = 6;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var fourthPosition = Position.Create(4).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // Act
        var result = volunteer.MovePet(secondPet, fourthPosition);

        // Assert
        result.IsSuccess.Should().BeTrue();

        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(4);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void MovePet_MoveForwardLastPetWhenNewPositionIsFirst_Succeeds()
    {
        // Arrange
        var petsCount = 6;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var firstPosition = Position.Create(1).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // Act
        var result = volunteer.MovePet(fifthPet, firstPosition);

        // Assert
        result.IsSuccess.Should().BeTrue();

        firstPet.Position.Value.Should().Be(2);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(5);
        fifthPet.Position.Value.Should().Be(1);
    }

    [Fact]
    public void MovePet_MoveBackFirstPetWhenNewPositionIsLast_Succeeds()
    {
        // Arrange
        var petsCount = 6;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var fifthPosition = Position.Create(5).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // Act
        var result = volunteer.MovePet(firstPet, fifthPosition);

        // Assert
        result.IsSuccess.Should().BeTrue();

        firstPet.Position.Value.Should().Be(5);
        secondPet.Position.Value.Should().Be(1);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(4);
    }
}
