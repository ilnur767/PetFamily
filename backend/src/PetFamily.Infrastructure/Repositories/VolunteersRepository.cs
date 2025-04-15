using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers.Commands;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

[UsedImplicitly]
public class VolunteersRepository : IVolunteersRepository
{
    private readonly WriteDbContext _context;

    public VolunteersRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _context.Volunteers.AddAsync(volunteer, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var volunteer = await _context.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (volunteer == null)
        {
            return Errors.General.NotFound(id);
        }

        return volunteer;
    }

    public async Task Save(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _context.Volunteers.Attach(volunteer);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid> HardDelete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _context.Volunteers.Remove(volunteer);
        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id.Value;
    }
}
