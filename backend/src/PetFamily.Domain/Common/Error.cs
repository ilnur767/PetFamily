﻿namespace PetFamily.Domain.Common;

public record Error
{
    private const string Separator = "||";

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; set; }
    public ErrorType Type { get; set; }

    public static Error Validation(string code, string message)
    {
        return new Error(code, message, ErrorType.Validation);
    }

    public static Error NotFound(string code, string message)
    {
        return new Error(code, message, ErrorType.NotFound);
    }

    public static Error Failure(string code, string message)
    {
        return new Error(code, message, ErrorType.Failure);
    }

    public static Error Conflict(string code, string message)
    {
        return new Error(code, message, ErrorType.Conflict);
    }

    public string Serialize()
    {
        return string.Join(Separator, Code, Message, Type);
    }

    public static Error Serialize(string serialized)
    {
        var parts = serialized.Split(Separator);

        if (parts.Length < 2)
        {
            throw new ArgumentException("Invalid serialized format.", serialized);
        }

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
        {
            throw new ArgumentException("Invalid serialized format.", serialized);
        }

        return new Error(parts[0], parts[1], type);
    }
}

public enum ErrorType
{
    Validation,
    NotFound,
    Failure,
    Conflict
}