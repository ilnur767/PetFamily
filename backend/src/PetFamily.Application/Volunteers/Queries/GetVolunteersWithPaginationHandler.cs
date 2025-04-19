using JetBrains.Annotations;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volunteers.Queries;

[UsedImplicitly]
public class GetVolunteersWithPaginationHandler : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetVolunteersWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<VolunteerDto>> Handle(GetVolunteersWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var volunteersQuery = _context.Volunteers.Where(v => v.IsDeleted == false);

        return await volunteersQuery.ToPagedListAsync(query.Page, query.PageSize, cancellationToken);
    }
}

public record GetVolunteersWithPaginationQuery(int Page, int PageSize) : IQuery;
