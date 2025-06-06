using JetBrains.Annotations;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Specieses.Application.Queries;

[UsedImplicitly]
public sealed class GetBreedsBySpeciesIdWithPaginationHandler : IQueryHandler<PagedList<BreedDto>, GetBreedsBySpeiesIdWithPaginationQuery>
{
    private readonly ISpeciesReadDbContext _context;

    public GetBreedsBySpeciesIdWithPaginationHandler(ISpeciesReadDbContext context)
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
