using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Files.AddFile;

public class AddFileHandler
{
    private readonly IFileProvider _fileProvider;

    public AddFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(AddFileCommand fileCommand, CancellationToken cancellationToken)
    {
        var result = await _fileProvider.UploadFile(
            new FileData(fileCommand.Stream, fileCommand.BucketName, fileCommand.FileName), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return result.Value;
    }
}

public record AddFileCommand(Stream Stream, string FileName, string BucketName);