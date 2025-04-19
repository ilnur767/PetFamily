using PetFamily.Application.Dtos;

namespace PetFamily.Application.Database;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
    IQueryable<SpeciesDto> Specieses { get; }
    IQueryable<BreedDto> Breeds { get; }
}
