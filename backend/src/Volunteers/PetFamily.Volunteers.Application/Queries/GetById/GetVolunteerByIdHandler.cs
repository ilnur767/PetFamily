using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Volunteers.Application.Queries.GetById;

[UsedImplicitly]
public sealed class GetVolunteerByIdHandler : IQueryHandler<VolunteerDto?, GetVolunteerByIdQuery>
{
    private readonly IVolunteerReadDbContext _context;

    public GetVolunteerByIdHandler(IVolunteerReadDbContext context)
    {
        _context = context;
    }

    public async Task<VolunteerDto?> Handle(GetVolunteerByIdQuery query, CancellationToken cancellationToken)
    {
        var volunteer = await _context.Volunteers
            .FirstOrDefaultAsync(v => v.Id == query.Id && v.IsDeleted == false, cancellationToken);

        return volunteer;
    }
}

public record GetVolunteerByIdQuery(Guid Id) : IQuery;
