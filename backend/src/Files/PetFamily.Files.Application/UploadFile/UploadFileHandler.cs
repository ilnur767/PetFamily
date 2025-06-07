using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Files.Application.FileProvider;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Files.Application.UploadFile;

public class UploadFileHandler : ICommandHandler<string, AddFileCommand>
{
    private readonly IFileProvider _fileProvider;

    public UploadFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, ErrorList>> Handle(AddFileCommand fileCommand, CancellationToken cancellationToken)
    {
        var result = await _fileProvider.UploadFile(
            new FileDataDto(fileCommand.Stream, new FileInfoDto(fileCommand.FileName, fileCommand.BucketName)), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        return result.Value;
    }
}

public record AddFileCommand(Stream Stream, string FileName, string BucketName) : ICommand;
