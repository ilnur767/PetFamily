using Microsoft.Extensions.DependencyInjection;
using PetFamily.Specieses.Contracts;

namespace PetFamily.Specieses.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesContract(this IServiceCollection services)
    {
        return services.AddScoped<ISpeciesContract, SpeciesContract>();
    }
}
