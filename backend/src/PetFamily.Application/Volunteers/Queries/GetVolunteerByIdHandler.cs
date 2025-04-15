using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Queries;

[UsedImplicitly]
public sealed class GetVolunteerByIdHandler : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly IReadDbContext _context;

    public GetVolunteerByIdHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<VolunteerDto> Handle(GetVolunteerByIdQuery query, CancellationToken cancellationToken)
    {
        var volunteer = await _context.Volunteers
            .FirstOrDefaultAsync(v => v.Id == query.Id && v.IsDeleted == false, cancellationToken);

        return volunteer;
    }
}

public record GetVolunteerByIdQuery(Guid Id) : IQuery;
