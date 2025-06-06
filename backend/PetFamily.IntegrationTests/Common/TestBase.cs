using AutoFixture;
using Microsoft.EntityFrameworkCore;
using PetFamily.Specieses.Infrastructure.DbContexts;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using Xunit;
using IServiceScope = Microsoft.Extensions.DependencyInjection.IServiceScope;
using ServiceProviderServiceExtensions = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

namespace PetFamily.IntegrationTests.Common;

public abstract class TestBase : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsWebFactory Factory;
    protected readonly Fixture Fixture = new();
    protected readonly IServiceScope Scope;
    protected readonly SpeciesWriteDbContext SpeciesWriteDbContext;
    protected readonly VolunteerWriteDbContext VolunteerWriteDbContext;

    protected TestBase(IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = ServiceProviderServiceExtensions.CreateScope(factory.Services);
        VolunteerWriteDbContext = ServiceProviderServiceExtensions.GetRequiredService<VolunteerWriteDbContext>(Scope.ServiceProvider);
        SpeciesWriteDbContext = ServiceProviderServiceExtensions.GetRequiredService<SpeciesWriteDbContext>(Scope.ServiceProvider);
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

        await VolunteerWriteDbContext.Volunteers.AddAsync(volunteer);
        await VolunteerWriteDbContext.SaveChangesAsync();

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

        await VolunteerWriteDbContext.AddRangeAsync(volunteers);
        await VolunteerWriteDbContext.SaveChangesAsync();

        return volunteers.Select(v => v.Id.Value).ToArray();
    }

    protected async Task<(Guid SpeciesId, Guid BreedId)> SeedSpecies()
    {
        var species = Fixture.CreateSpecies();

        var breed = Fixture.CreateBreed();
        species.AddBreed(breed);

        await SpeciesWriteDbContext.AddAsync(species);
        await SpeciesWriteDbContext.SaveChangesAsync();

        return (species.Id.Value, breed.Id.Value);
    }

    protected async Task<Guid> SeedPet(Guid? volunteerId = null, Guid? speciesId = null, Guid? breedId = null)
    {
        var pet = Fixture.CreatePet(speciesId, breedId);

        var volunteer = await VolunteerWriteDbContext.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteerId);

        if (volunteer == null)
        {
            var id = await SeedVolunteer();
            volunteer = await VolunteerWriteDbContext.Volunteers.FirstAsync(v => v.Id == id);
        }

        volunteer.AddPet(pet);
        await VolunteerWriteDbContext.SaveChangesAsync();

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
