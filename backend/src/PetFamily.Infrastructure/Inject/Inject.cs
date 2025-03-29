using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.Common;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Application.Species;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.Files;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;
using PetFamily.Infrastructure.Services;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;
using MinioOptions = PetFamily.Infrastructure.Options.MinioOptions;

namespace PetFamily.Infrastructure.Inject;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SchedulingOptions>(configuration.GetSection(nameof(SchedulingOptions)));
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        services.AddScoped<DeleteExpiredVolunteersService>();
        services.AddScoped<IFileCleanerService, FileCleanerService>();

        services.AddBackgroundServices();

        services.AddMinio(configuration);

        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();

        return services;
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<DeleteExpiredEntitiesService>();
        services.AddHostedService<FilesCleanerBackgroundService>();

        return services;
    }

    private static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(nameof(MinioOptions)));

        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(nameof(MinioOptions)).Get<MinioOptions>() ??
                               throw new ApplicationException("Missing minio configuration");

            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        return services;
    }
}
