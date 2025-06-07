using PetFamily.Core.Dtos;

namespace PetFamily.Volunteers.Application;

public interface IVolunteerReadDbContext
{
    public IQueryable<VolunteerDto> Volunteers { get; }
    public IQueryable<PetDto> Pets { get; }
}
