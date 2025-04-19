using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands;

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

    /// <summary>
    ///     Получение волонтера по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор волонтера.</param>
    /// <param name="cancellationToken">Токен отменмы.</param>
    /// <returns>Сущность волонетера.</returns>
    public Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Сохранение состояния волонтера.
    /// </summary>
    /// <param name="volunteer">Сущность волонетера.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор волонтера.</returns>
    public Task Save(Volunteer volunteer, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Полное удаление волонтера.
    /// </summary>
    /// <param name="volunteer">Сущность волонетера.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор волонтера.</returns>
    public Task<Guid> HardDelete(Volunteer volunteer, CancellationToken cancellationToken = default);
}
