using System.Data.Common;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using NSubstitute;
using PetFamily.Core.Dtos;
using PetFamily.Files.Application.FileProvider;
using PetFamily.SharedKernel.Common;
using PetFamily.Specieses.Infrastructure.DbContexts;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using PetFamily.Web;
using Respawn;
using Testcontainers.PostgreSql;
using Xunit;

namespace PetFamily.IntegrationTests.Common;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_family")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly IFileProvider _fileProviderMock = Substitute.For<IFileProvider>();

    private DbConnection _dbConnection;

    private Respawner _respawner;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();

        var volunteerDbContext = scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
        await volunteerDbContext.Database.MigrateAsync();

        var speciesDbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
        await speciesDbContext.Database.MigrateAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());

        await InitializeRespawner();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    public void SetupSuccessFileProviderMock()
    {
        _fileProviderMock
            .UploadFiles(Arg.Any<IEnumerable<FileDataDto>>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success<IReadOnlyCollection<string>, Error>(["filePath"]));
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices);
        builder.ConfigureAppConfiguration(ConfigureConfigs);
    }

    private void ConfigureConfigs(WebHostBuilderContext context, IConfigurationBuilder builder)
    {
        builder.AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:Database"] = _dbContainer.GetConnectionString() });
        builder.AddInMemoryCollection(new Dictionary<string, string?> { ["ADMIN:USERNAME"] = "admin" });
        builder.AddInMemoryCollection(new Dictionary<string, string?> { ["ADMIN:EMAIL"] = "admin123@admin.com" });
        builder.AddInMemoryCollection(new Dictionary<string, string?> { ["ADMIN:PASSWORD"] = "ADSFdfdsfdsdf@123" });
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        services.RemoveAll(typeof(IFileProvider));
        services.AddScoped<IFileProvider>(_ => _fileProviderMock);
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions { DbAdapter = DbAdapter.Postgres });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
}
