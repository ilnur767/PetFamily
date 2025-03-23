using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;

namespace PetFamily.Application.FileProvider;

public interface IFileProvider
{
    /// <summary>
    ///     Загрузить файл в хранилище.
    /// </summary>
    /// <param name="fileData">Информация о файле.</param>
    /// <param name="bucketName">Наименование бакета.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Наименование файла в хранилище.</returns>
    public Task<Result<string, Error>> UploadFile(FileData fileData, string bucketName, CancellationToken cancellationToken);

    /// <summary>
    ///     Загрузить файл в хранилище.
    /// </summary>
    /// <param name="filesData">Информация о файлах.</param>
    /// <param name="bucketName">Наименование бакета.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Наименование файла в хранилище.</returns>
    public Task<Result<IReadOnlyCollection<string>, Error>>
        UploadFiles(IEnumerable<FileData> filesData, string bucketName, CancellationToken cancellationToken);

    /// <summary>
    ///     Получить ссылку на скачивание файла из хранилища.
    /// </summary>
    /// <param name="fileName">Наименование файла.</param>
    /// <param name="bucketName">Наименование бакета.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ссылка на скачивание файла.</returns>
    public Task<Result<string, Error>> GetPresignedUrl(string fileName, string bucketName,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Удалить файл из хранилища.
    /// </summary>
    /// <param name="fileName">Наименование файла.</param>
    /// <param name="bucketName">Наименование бакета.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task<UnitResult<Error>> DeleteFile(string fileName, string bucketName, CancellationToken cancellationToken);

    /// <summary>
    ///     Удалить файл из хранилища.
    /// </summary>
    /// <param name="filesPath">Наименование файла.</param>
    /// <param name="bucketName">Наименование бакета.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task<UnitResult<Error>> DeleteFiles(IEnumerable<string> filesPath, string bucketName, CancellationToken cancellationToken);
}
