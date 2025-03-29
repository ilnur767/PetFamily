using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Infrastructure.Files;

public sealed class FileCleanerService : IFileCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public FileCleanerService(IMessageQueue<IEnumerable<FileInfo>> messageQueue, IFileProvider fileProvider)
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
