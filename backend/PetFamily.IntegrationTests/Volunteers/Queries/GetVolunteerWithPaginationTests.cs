using FluentAssertions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using PetFamily.IntegrationTests.Common;
using PetFamily.Volunteers.Application.Queries.GetWithPagination;
using Xunit;

namespace PetFamily.IntegrationTests.Volunteers.Queries;

public sealed class GetVolunteerWithPaginationTests : QueryTestBase<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
{
    public GetVolunteerWithPaginationTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Handle_GetVolunteersWithPagination_Success()
    {
        // Arrange
        var pageSize = 5;
        await SeedVolunteers(10);

        var query = new GetVolunteersWithPaginationQuery(1, pageSize);

        // Act
        var pagedResult = await Sut.Handle(query, CancellationToken.None);

        // Assert
        pagedResult.Items.Count.Should().Be(pageSize);
    }
}
