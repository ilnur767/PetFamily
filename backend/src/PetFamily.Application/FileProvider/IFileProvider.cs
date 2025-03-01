using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;

namespace PetFamily.Application.FileProvider;

public interface IFileProvider
{
    /// <summary>
    /// </summary>
    /// <param name="fileData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<string, Error>> UploadFile(FileData fileData, CancellationToken cancellationToken);

    public Task<Result<string, Error>> GetPresignedUrl(string fileName, string bucketName,
        CancellationToken cancellationToken);

    public Task<UnitResult<Error>> DeleteFile(string fileName, string bucketName, CancellationToken cancellationToken);
}