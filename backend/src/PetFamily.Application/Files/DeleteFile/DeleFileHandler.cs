using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Files.DeleteFile;

public sealed class DeleteFileHandler
{
    private readonly IFileProvider _fileProvider;

    public DeleteFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<UnitResult<Error>> Handle(DeleteFileCommand command, CancellationToken cancellationToken)
    {
        var result = await _fileProvider.DeleteFile(command.FileName, command.BucketName, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return new UnitResult<Error>();
    }
}

public record DeleteFileCommand(string FileName, string BucketName);