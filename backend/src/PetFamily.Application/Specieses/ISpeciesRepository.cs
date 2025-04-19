using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;
using PetFamily.Domain.Specieses;

namespace PetFamily.Application.Specieses;

public interface ISpeciesRepository
{
    /// <summary>
    ///     Получить вид животного по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор вида.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task<Result<Species, Error>> GetById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    ///     Создать вид животного.
    /// </summary>
    /// <param name="species">Вид.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task<Result<Guid, Error>> Add(Species species, CancellationToken cancellationToken);

    /// <summary>
    ///     Сохранить состояние вида.
    /// </summary>
    /// <param name="species">Вид.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task Save(Species species, CancellationToken cancellationToken);
}
