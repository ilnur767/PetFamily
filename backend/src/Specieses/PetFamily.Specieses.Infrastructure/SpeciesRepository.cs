using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel.Common;
using PetFamily.Specieses.Application;
using PetFamily.Specieses.Domain.Specieses;
using PetFamily.Specieses.Infrastructure.DbContexts;

namespace PetFamily.Specieses.Infrastructure;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly SpeciesWriteDbContext _context;

    public SpeciesRepository(SpeciesWriteDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Species, Error>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var species = await _context.Specieses
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted == false, cancellationToken);

        if (species == null)
        {
            return Errors.General.NotFound(id);
        }

        return species;
    }

    public async Task<Result<Guid, Error>> Add(Species species, CancellationToken cancellationToken)
    {
        await _context.Specieses
            .AddAsync(species, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return species.Id.Value;
    }

    public async Task Save(Species species, CancellationToken cancellationToken)
    {
        _context.Specieses.Attach(species);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
