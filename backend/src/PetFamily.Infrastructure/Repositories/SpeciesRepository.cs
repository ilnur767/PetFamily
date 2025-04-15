using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Common;
using PetFamily.Domain.Specieses;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly WriteDbContext _context;

    public SpeciesRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Species, Error>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var species = await _context.Specieses
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (species == null)
        {
            return Errors.General.NotFound(id);
        }

        return species;
    }
}
