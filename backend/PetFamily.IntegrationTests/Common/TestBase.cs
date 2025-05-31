using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NSubstitute.Exceptions;
using PetFamily.Domain.Volunteers;
using PetFamily.Infrastructure.DbContexts;
using Xunit;
using IServiceScope = Microsoft.Extensions.DependencyInjection.IServiceScope;
using ServiceProviderServiceExtensions = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

namespace PetFamily.IntegrationTests.Common;

public abstract class TestBase : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly WriteDbContext DbContext;
    protected readonly IntegrationTestsWebFactory Factory;
    protected readonly Fixture Fixture = new();
    protected readonly IServiceScope Scope;

    protected TestBase(IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = ServiceProviderServiceExtensions.CreateScope(factory.Services);
        DbContext = ServiceProviderServiceExtensions.GetRequiredService<WriteDbContext>(Scope.ServiceProvider);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await Factory.ResetDatabaseAsync();
    }

    protected async Task<Guid> SeedVolunteer()
    {
        var volunteer = Fixture.CreateVolunteer();

        await DbContext.Volunteers.AddAsync(volunteer);
        await DbContext.SaveChangesAsync();

        return volunteer.Id;
    }

    protected async Task<Guid[]> SeedVolunteers(int count)
    {
        var volunteers = new List<Volunteer>();
        for (var i = 0; i < count; i++)
        {
            var volunteer = Fixture.CreateVolunteer();
            volunteers.Add(volunteer);
        }

        await DbContext.Volunteers.AddRangeAsync(volunteers);
        await DbContext.SaveChangesAsync();

        return volunteers.Select(v => v.Id.Value).ToArray();
    }

    protected async Task<(Guid SpeciesId, Guid BreedId)> SeedSpecies()
    {
        var species = Fixture.CreateSpecies();

        var breed = Fixture.CreateBreed();
        species.AddBreed(breed);

        await DbContext.Specieses.AddAsync(species);
        await DbContext.SaveChangesAsync();

        return (species.Id.Value, breed.Id.Value);
    }

    protected async Task<Guid> SeedPet(Guid volunteerId, Guid speciesId, Guid breedId)
    {
        var pet = Fixture.CreatePet(volunteerId, speciesId, breedId);

        var volunteer = await DbContext.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteerId);

        if (volunteer == null)
        {
            throw new ArgumentNotFoundException("Volunteer not found");
        }

        volunteer.AddPet(pet);
        await DbContext.SaveChangesAsync();

        return pet.Id.Value;
    }
}
