using JetBrains.Annotations;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Volunteers.Application.Queries.GetWithPagination;

[UsedImplicitly]
public class GetVolunteersWithPaginationHandler : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
{
    private readonly IVolunteerReadDbContext _context;

    public GetVolunteersWithPaginationHandler(IVolunteerReadDbContext context)
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
