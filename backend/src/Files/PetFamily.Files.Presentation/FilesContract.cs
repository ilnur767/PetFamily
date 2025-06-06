using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Files.Application.DeleteFiles;
using PetFamily.Files.Application.UploadFile;
using PetFamily.Files.Contracts;
using PetFamily.Files.Contracts.Requests;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Files.Presentation;

public class FilesContract : IFilesContract
{
    private readonly ICommandHandler<DeleteFilesCommand> _deleteFilesCommandHandler;
    private readonly ICommandHandler<IReadOnlyCollection<string>, UploadFilesCommand> _uploadFilesCommandHandler;

    public FilesContract(ICommandHandler<DeleteFilesCommand> deleteFilesCommandHandler,
        ICommandHandler<IReadOnlyCollection<string>, UploadFilesCommand> uploadFilesCommandHandler)
    {
        _deleteFilesCommandHandler = deleteFilesCommandHandler;
        _uploadFilesCommandHandler = uploadFilesCommandHandler;
    }

    public async Task<UnitResult<ErrorList>> DeleteFiles(DeleteFileRequest request, CancellationToken cancellationToken)
    {
        return await _deleteFilesCommandHandler.Handle(new DeleteFilesCommand(request.FileName, request.BucketName), cancellationToken);
    }

    public async Task<Result<IReadOnlyCollection<string>, ErrorList>> UploadFiles(UploadFilesRequest request, CancellationToken cancellationToken)
    {
        return await _uploadFilesCommandHandler.Handle(new UploadFilesCommand(request.FilesData, request.BucketName), cancellationToken);
    }
}
