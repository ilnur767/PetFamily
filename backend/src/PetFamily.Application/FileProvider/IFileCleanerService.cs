namespace PetFamily.Application.FileProvider;

/// <summary>
///     Сервис для очистки файлов из хранилища.
/// </summary>
public interface IFileCleanerService
{
    /// <summary>
    ///     Инициирует запуск задачи очистки файлов из хранилища.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task Process(CancellationToken cancellationToken);
}
