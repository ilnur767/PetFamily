using AutoFixture;
using Microsoft.EntityFrameworkCore;
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

    protected async Task<Guid> SeedPet(Guid? volunteerId = null, Guid? speciesId = null, Guid? breedId = null)
    {
        var pet = Fixture.CreatePet(speciesId, breedId);

        var volunteer = await DbContext.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteerId);

        if (volunteer == null)
        {
            var id = await SeedVolunteer();
            volunteer = await DbContext.Volunteers.FirstAsync(v => v.Id == id);
        }

        volunteer.AddPet(pet);
        await DbContext.SaveChangesAsync();

        return pet.Id.Value;
    }

    protected async Task<Guid[]> SeedPets(int count)
    {
        var ids = new List<Guid>();
        for (var i = 0; i < count; i++)
        {
            var id = await SeedPet();
            ids.Add(id);
        }

        return ids.ToArray();
    }
}
