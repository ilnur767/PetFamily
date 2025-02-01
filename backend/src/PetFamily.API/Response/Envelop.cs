using PetFamily.Domain.Common;

namespace PetFamily.API.Response;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelop
{
    private Envelop(object? result, IEnumerable<ResponseError> errors)
    {
        Result = result;
        Errors = errors.ToList();
        CreatedAt = DateTime.UtcNow;
    }

    public object? Result { get; }

    public List<ResponseError> Errors { get; }
    
    public DateTimeOffset CreatedAt { get; }

    public static Envelop Ok(object? result)
    {
        return new Envelop(result, []);
    }

    public static Envelop Error(IEnumerable<ResponseError> errors)
    {
        return new Envelop(null, errors);
    }
}