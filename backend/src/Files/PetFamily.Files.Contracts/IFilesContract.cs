using CSharpFunctionalExtensions;
using PetFamily.Files.Contracts.Requests;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Files.Contracts;

public interface IFilesContract
{
    Task<UnitResult<ErrorList>> DeleteFiles(DeleteFileRequest request, CancellationToken cancellationToken);
    Task<Result<IReadOnlyCollection<string>, ErrorList>> UploadFiles(UploadFilesRequest request, CancellationToken cancellationToken);
}
