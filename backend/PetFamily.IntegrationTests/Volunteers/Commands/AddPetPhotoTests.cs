using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PetFamily.IntegrationTests.Common;
using PetFamily.IntegrationTests.Helpers;
using PetFamily.Volunteers.Application.Commands.AddPetPhoto;
using Xunit;

namespace PetFamily.IntegrationTests.Volunteers.Commands;

public class AddPetPhotoTests : CommandTestBase<IReadOnlyCollection<string>, AddPetPhotoCommand>
{
    public AddPetPhotoTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Handle_AddPetPhoto_Success()
    {
        // Arrange
        var volunteerId = await SeedVolunteer();
        var (speiesId, breedId) = await SeedSpecies();
        var petId = await SeedPet(volunteerId, speiesId, breedId);
        Factory.SetupSuccessFileProviderMock();

        var command = new AddPetPhotoCommand(volunteerId, petId, [new UploadPhotoDto(new TestStream(), Fixture.Create<string>())]);

        // Act
        var result = await Sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var volunteer = await VolunteerWriteDbContext.Volunteers
            .FirstOrDefaultAsync();

        volunteer.Should().NotBeNull();
        var pet = volunteer!.Pets.FirstOrDefault();
        pet.Should().NotBeNull();

        pet!.Photos.Should().NotBeNull();
        pet!.Photos.Single().FileName.Should().BeEquivalentTo(command.Photos.Single().FileName);
    }
}
