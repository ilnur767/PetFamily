using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PetFamily.IntegrationTests.Common;
using PetFamily.Volunteers.Application.Commands.Create;
using Xunit;

namespace PetFamily.IntegrationTests.Volunteers.Commands;

public class AddVolunteerTests : CommandTestBase<Guid, CreateVolunteerCommand>
{
    public AddVolunteerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void Handle_CreateVolunteerCommand_Success()
    {
        // Arrange
        var command = Fixture.CreateVolunteerCommand();

        // Act
        var result = await Sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var volunteer = await VolunteerWriteDbContext.Volunteers.FirstOrDefaultAsync();
        volunteer.Should().NotBeNull();
    }
}
