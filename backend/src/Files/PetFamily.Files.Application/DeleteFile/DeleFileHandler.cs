using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Files.Application.FileProvider;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Files.Application.DeleteFile;

public sealed class DeleteFileHandler : ICommandHandler<DeleteFileCommand>
{
    private readonly IFileProvider _fileProvider;

    public DeleteFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<UnitResult<ErrorList>> Handle(DeleteFileCommand command, CancellationToken cancellationToken)
    {
        var result = await _fileProvider.DeleteFile(new FileInfoDto(command.FileName, command.BucketName), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        return new UnitResult<ErrorList>();
    }
}

public record DeleteFileCommand(string FileName, string BucketName) : ICommand;
