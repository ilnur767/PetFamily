using JetBrains.Annotations;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volunteers.Queries.GetPetsWithPagination;

[UsedImplicitly]
public sealed class GetPetsWithPaginationHandler : IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetPetsWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Получить питомцев постранично с фильтрацией и сортировкой.
    /// </summary>
    /// <param name="query">Тело запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Постраничный список питомцев.</returns>
    public async Task<PagedList<PetDto>> Handle(GetPetsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        var petsQuery = _context.Pets.Where(p => p.IsDeleted == false);

        petsQuery = FilterPets(query, petsQuery);

        petsQuery = SortPets(petsQuery);

        return await petsQuery.ToPagedListAsync(query.Page, query.PageSize, cancellationToken);
    }

    private IQueryable<PetDto> SortPets(IQueryable<PetDto> query)
    {
        return query
            .OrderBy(q => q.NickName)
            .ThenBy(q => q.BreedId)
            .ThenBy(q => q.Color)
            .ThenBy(q => q.Address)
            .ThenBy(q => q.VolunteerId);
    }

    private static IQueryable<PetDto> FilterPets(GetPetsWithPaginationQuery query, IQueryable<PetDto> petsQuery)
    {
        petsQuery = petsQuery.WhereIf(query.VolunteerIds?.Length > 0, p => query.VolunteerIds != null && query.VolunteerIds.Contains(p.VolunteerId));
        petsQuery = petsQuery.WhereIf(!string.IsNullOrEmpty(query.NickName), p => p.NickName.Contains(query.NickName!));
        petsQuery = petsQuery.WhereIf(!string.IsNullOrEmpty(query.Description), p => p.Description.Contains(query.Description!));
        petsQuery = petsQuery.WhereIf(!string.IsNullOrEmpty(query.Color), p => p.Color.Contains(query.Color!));
        petsQuery = petsQuery.WhereIf(!string.IsNullOrEmpty(query.HealthInformation), p => p.HealthInformation.Contains(query.HealthInformation!));
        petsQuery = petsQuery.WhereIf(!string.IsNullOrEmpty(query.Address), p => p.Address.Contains(query.Address!));
        petsQuery = petsQuery.WhereIf(query.Weight != null, p => p.Weight == query.Weight);
        petsQuery = petsQuery.WhereIf(query.Height != null, p => p.Height == query.Height);
        petsQuery = petsQuery.WhereIf(!string.IsNullOrEmpty(query.PhoneNumber), p => p.PhoneNumber.Contains(query.PhoneNumber!));
        petsQuery = petsQuery.WhereIf(query.IsCastrated != null, p => p.IsCastrated == query.IsCastrated);
        petsQuery = petsQuery.WhereIf(query.DateOfBirth != null, p => p.DateOfBirth == query.DateOfBirth);
        petsQuery = petsQuery.WhereIf(query.IsVaccinated != null, p => p.IsVaccinated == query.IsVaccinated);

        return petsQuery;
    }
}

public record GetPetsWithPaginationQuery(
    Guid[]? VolunteerIds,
    string? NickName,
    string? Description,
    string? Color,
    string? HealthInformation,
    string? Address,
    double? Weight,
    double? Height,
    string? PhoneNumber,
    bool? IsCastrated,
    DateTime? DateOfBirth,
    bool? IsVaccinated,
    int Page,
    int PageSize) : IQuery;
