using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Core.Dtos;
using PetFamily.Core.Messaging;
using PetFamily.Files.Application.FileProvider;
using PetFamily.Files.Infrastructure.BackgroundServices;
using PetFamily.Files.Infrastructure.MessageQueues;
using PetFamily.Files.Infrastructure.Options;

namespace PetFamily.Files.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFilesInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFileCleanerService, FileCleanerService>();
        services.AddScoped<IFileProvider, MinioProvider>();
        services.AddHostedService<FilesCleanerBackgroundService>();
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfoDto>>, InMemoryMessageQueue<IEnumerable<FileInfoDto>>>();
        services.AddMinio(configuration);

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


        return services;
    }
}
