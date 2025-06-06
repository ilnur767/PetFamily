using System.Threading.Channels;
using PetFamily.Core.Messaging;

namespace PetFamily.Files.Infrastructure.MessageQueues;

public class InMemoryMessageQueue<TMessage> : IMessageQueue<TMessage>
{
    private readonly Channel<TMessage> _channel = Channel.CreateUnbounded<TMessage>();

    public async Task WriteAsync(TMessage files, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(files, cancellationToken);
    }

    public async Task<TMessage> ReadAsync(CancellationToken cancellationToken)
    {
        var result = await _channel.Reader.ReadAsync(cancellationToken);

        return result;
    }
}
