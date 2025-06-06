using CSharpFunctionalExtensions;
using PetFamily.Core.Dtos;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Files.Application.FileProvider;

public interface IFileProvider
{
    /// <summary>
    ///     Загрузить файл в хранилище.
    /// </summary>
    /// <param name="fileDataDto">Информация о файле.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Наименование файла в хранилище.</returns>
    public Task<Result<string, Error>> UploadFile(FileDataDto fileDataDto, CancellationToken cancellationToken);

    /// <summary>
    ///     Загрузить файл в хранилище.
    /// </summary>
    /// <param name="filesData">Информация о файлах.</param>
    /// <param name="bucketName">Наименование бакета.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Наименование файла в хранилище.</returns>
    public Task<Result<IReadOnlyCollection<string>, Error>>
        UploadFiles(IEnumerable<FileDataDto> filesData, string bucketName, CancellationToken cancellationToken);

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
    /// <param name="file">Информация о файле.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task<UnitResult<Error>> DeleteFile(FileInfoDto file, CancellationToken cancellationToken);

    /// <summary>
    ///     Удалить файл из хранилища.
    /// </summary>
    /// <param name="filesPath">Наименование файла.</param>
    /// <param name="bucketName">Наименование бакета.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task<UnitResult<Error>> DeleteFiles(IEnumerable<string> filesPath, string bucketName, CancellationToken cancellationToken);
}
