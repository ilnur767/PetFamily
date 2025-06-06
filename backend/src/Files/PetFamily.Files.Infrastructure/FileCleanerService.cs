using PetFamily.Core.Dtos;
using PetFamily.Core.Messaging;
using PetFamily.Files.Application.FileProvider;

namespace PetFamily.Files.Infrastructure;

public sealed class FileCleanerService : IFileCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly IMessageQueue<IEnumerable<FileInfoDto>> _messageQueue;

    public FileCleanerService(IMessageQueue<IEnumerable<FileInfoDto>> messageQueue, IFileProvider fileProvider)
    {
        _messageQueue = messageQueue;
        _fileProvider = fileProvider;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var filesInfo = await _messageQueue.ReadAsync(cancellationToken);

        foreach (var fileInfo in filesInfo)
        {
            await _fileProvider.DeleteFile(fileInfo, cancellationToken);
        }
    }
}
