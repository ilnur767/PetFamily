using PetFamily.Domain.Common;

namespace PetFamily.API.Response;

public record Envelop
{
    private Envelop(object? result, Error? error)
    {
        Result = result;
        ErrorCode = error?.Code;
        ErrorMessage = error?.Message;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public object? Result { get; }

    public string? ErrorCode { get; }

    public string? ErrorMessage { get; }

    public DateTimeOffset CreatedAt { get; }

    public static Envelop Ok(object? result)
    {
        return new Envelop(result, null);
    }

    public static Envelop Error(Error error)
    {
        return new Envelop(null, error);
    }
}