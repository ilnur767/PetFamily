using FluentAssertions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Queries.GetPetById;
using PetFamily.IntegrationTests.Common;
using Xunit;

namespace PetFamily.IntegrationTests.Volunteers.Queries;

public sealed class GetPetByIdTests : QueryTestBase<PetDto?, GetPetByIdQuery>
{
    public GetPetByIdTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Handle_GetPetById_Succeess()
    {
        // Arrange
        var ids = await SeedPets(5);

        var id = ids.First();

        var query = new GetPetByIdQuery(id);

        // Act
        var result = await Sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
    }
}
