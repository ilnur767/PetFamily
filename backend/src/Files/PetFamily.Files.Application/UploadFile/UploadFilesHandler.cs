using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Files.Application.FileProvider;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Files.Application.UploadFile;

public class UploadFilesHandler : ICommandHandler<IReadOnlyCollection<string>, UploadFilesCommand>
{
    private readonly IFileProvider _fileProvider;

    public UploadFilesHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<IReadOnlyCollection<string>, ErrorList>> Handle(UploadFilesCommand fileCommand, CancellationToken cancellationToken)
    {
        var result = await _fileProvider.UploadFiles(fileCommand.FilesData, fileCommand.BucketName, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        return Result.Success<IReadOnlyCollection<string>, ErrorList>(result.Value);
    }
}

public record UploadFilesCommand(IEnumerable<FileDataDto> FilesData, string BucketName) : ICommand;
