using PetFamily.Domain.Common;

namespace PetFamily.API.Response;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelop
{
    private Envelop(object? result, ErrorList errors)
    {
        Result = result;
        Errors = errors;
        CreatedAt = DateTime.UtcNow;
    }

    public object? Result { get; }

    public ErrorList? Errors { get; }

    public DateTimeOffset CreatedAt { get; }

    public static Envelop Ok(object? result)
    {
        return new Envelop(result, null);
    }

    public static Envelop Error(ErrorList errors)
    {
        return new Envelop(null, errors);
    }

    public static Envelop Error(Error error)
    {
        return new Envelop(null, error.ToErrorList());
    }
}
