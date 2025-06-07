using Microsoft.Extensions.DependencyInjection;
using PetFamily.Files.Contracts;

namespace PetFamily.Files.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddFilesContract(this IServiceCollection services)
    {
        services.AddScoped<IFilesContract, FilesContract>();

        return services;
    }
}
