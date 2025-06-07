using JetBrains.Annotations;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Specieses.Application.Queries;

[UsedImplicitly]
public sealed class GetSpeiciesWithPaginationHandler : IQueryHandler<PagedList<SpeciesDto>, GetSpeiciesWithPaginationQuery>
{
    private readonly ISpeciesReadDbContext _context;

    public GetSpeiciesWithPaginationHandler(ISpeciesReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<SpeciesDto>> Handle(GetSpeiciesWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var species = _context.Specieses;

        return await species.ToPagedListAsync(query.Page, query.PageSize, cancellationToken);
    }
}

public record GetSpeiciesWithPaginationQuery(int Page, int PageSize) : IQuery;
