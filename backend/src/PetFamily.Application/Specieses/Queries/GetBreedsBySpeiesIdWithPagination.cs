using JetBrains.Annotations;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Specieses.Queries;

[UsedImplicitly]
public sealed class GetBreedsBySpeciesIdWithPaginationHandler : IQueryHandler<PagedList<BreedDto>, GetBreedsBySpeiesIdWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetBreedsBySpeciesIdWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public Task<PagedList<BreedDto>> Handle(GetBreedsBySpeiesIdWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var breeds = _context.Breeds.Where(b => b.SpeciesId == query.SpeciesId);

        return breeds.ToPagedListAsync(query.Page, query.PageSize, cancellationToken);
    }
}

public record GetBreedsBySpeiesIdWithPaginationQuery(Guid SpeciesId, int Page, int PageSize) : IQuery;
