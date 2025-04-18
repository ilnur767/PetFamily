using JetBrains.Annotations;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Specieses.Queries;

[UsedImplicitly]
public sealed class GetSpeiciesWithPaginationHandler : IQueryHandler<PagedList<SpeciesDto>, GetSpeiciesWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetSpeiciesWithPaginationHandler(IReadDbContext context)
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
