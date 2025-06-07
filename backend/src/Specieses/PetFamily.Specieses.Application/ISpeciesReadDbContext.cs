using PetFamily.Core.Dtos;

namespace PetFamily.Specieses.Application;

public interface ISpeciesReadDbContext
{
    IQueryable<SpeciesDto> Specieses { get; }
    IQueryable<BreedDto> Breeds { get; }
}
