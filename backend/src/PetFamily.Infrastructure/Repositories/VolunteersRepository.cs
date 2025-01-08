﻿using JetBrains.Annotations;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Infrastructure.Repositories;

[UsedImplicitly]
public class VolunteersRepository : IVolunteersRepository
{
    private readonly ApplicationDbContext _context;

    public VolunteersRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _context.Volunteers.AddAsync(volunteer, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id.Value;
    }
}