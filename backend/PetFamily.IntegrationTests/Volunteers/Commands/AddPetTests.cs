using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PetFamily.IntegrationTests.Common;
using PetFamily.Volunteers.Application.Commands.AddPet;
using Xunit;

namespace PetFamily.IntegrationTests.Volunteers.Commands;

public class AddPetTests : CommandTestBase<Guid, AddPetCommand>
{
    public AddPetTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Handle_AddPet_Success()
    {
        // Arrange
        var volunteerId = await SeedVolunteer();
        var (speiesId, breedId) = await SeedSpecies();

        var command = Fixture.CreatePetCommand(volunteerId, speiesId, breedId);

        // Act
        var result = await Sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var volunteer = await VolunteerWriteDbContext.Volunteers
            .Include(volunteer => volunteer.Pets)
            .FirstOrDefaultAsync();

        volunteer.Should().NotBeNull();
        var pet = volunteer!.Pets.FirstOrDefault();
        pet.Should().NotBeNull();
    }
}
