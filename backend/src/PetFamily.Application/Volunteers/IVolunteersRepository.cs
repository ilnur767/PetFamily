using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers;

/// <summary>
///     Репозиторий волонтера.
/// </summary>
public interface IVolunteersRepository
{
    /// <summary>
    ///     Добавление волонтера.
    /// </summary>
    /// <param name="volunteer">Модель волонтера.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор добавленного волонтера.</returns>
    public Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
}