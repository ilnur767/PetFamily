using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Files.Application.FileProvider;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Files.Application.DeleteFiles;

public sealed class DeleteFilesHandler : ICommandHandler<DeleteFilesCommand>
{
    private readonly IFileProvider _fileProvider;

    public DeleteFilesHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<UnitResult<ErrorList>> Handle(DeleteFilesCommand command, CancellationToken cancellationToken)
    {
        var result = await _fileProvider.DeleteFiles(command.FileNames, command.BucketName, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        return new UnitResult<ErrorList>();
    }
}

public record DeleteFilesCommand(IEnumerable<string> FileNames, string BucketName) : ICommand;
