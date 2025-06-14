﻿namespace PetFamily.SharedKernel.Common;

public static class Errors
{
    public const string InvalidValueCode = "value.is.invalid";
    public const string RecordNotFoundCode = "record.not.found";
    public const string InternalServerErrorCode = "server.internal";

    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";

            return Error.Validation(InvalidValueCode, $"{label} is invalid");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";

            return Error.NotFound(RecordNotFoundCode, $"record not found{forId}");
        }

        public static Error NotFound(string message)
        {
            return Error.NotFound(RecordNotFoundCode, message);
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? "" : " " + name + " ";

            return Error.Validation(InvalidValueCode, $"invalid{label}length");
        }

        public static Error AlreadyExists()
        {
            return Error.NotFound("record.already.exists", "Record already exists");
        }
    }

    public static class User
    {
        public static Error InvalidCredentials()
        {
            return Error.Validation("credentials.is.invalid", "Your credentials is invalid");
        }
    }
}
