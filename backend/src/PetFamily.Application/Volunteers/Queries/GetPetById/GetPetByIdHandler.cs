using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Queries.GetPetById;

[UsedImplicitly]
public sealed class GetPetByIdHandler : IQueryHandler<PetDto?, GetPetByIdQuery>
{
    private readonly IReadDbContext _context;

    public GetPetByIdHandler(IReadDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Полчуить питомца по идентификаторую
    /// </summary>
    /// <param name="query">Тело запроса.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель питомца.</returns>
    public async Task<PetDto?> Handle(GetPetByIdQuery query, CancellationToken cancellationToken)
    {
        return await _context.Pets.FirstOrDefaultAsync(p => p.Id == query.Id && p.IsDeleted == false, cancellationToken);
    }
}

public record GetPetByIdQuery(Guid Id) : IQuery;
