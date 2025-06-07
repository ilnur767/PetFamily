using System.Reflection;
using PetFamily.Files.Application;
using PetFamily.Files.Infrastructure;
using PetFamily.Files.Presentation;
using PetFamily.Specieses.Application;
using PetFamily.Specieses.Infrastructure;
using PetFamily.Specieses.Presentation;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure;
using PetFamily.Volunteers.Presentation;
using Serilog;

namespace PetFamily.Web.Common;

public static class ServicesInstaller
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddLogger(configuration);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        services.AddControllers();

        services.AddHttpLogging(o => { });

        services
            .AddSpeciesesApplication()
            .AddSpeciesesInfrastructure()
            .AddSpeciesContract();

        services
            .AddVolunteersApplication(configuration)
            .AddVolunteersInfrastructure()
            .AddVolunteersContract();

        services
            .AddFilesApplication()
            .AddFilesInfrastructure(configuration)
            .AddFilesContract();

        services.AddSerilog();

        return services;
    }

    private static void AddLogger(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Seq(configuration.GetConnectionString("Seq") ?? throw new ArgumentNullException("Seq"))
            .CreateLogger();
    }
}
