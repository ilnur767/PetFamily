using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Framework;

public static class ResponseExtensions
{
    public static ActionResult ToErrorResponse(this Error error)
    {
        var statusCode = GetStatusCode(error);

        var envelope = Envelop.Error(error);

        return new ObjectResult(envelope) { StatusCode = statusCode };
    }

    public static ActionResult ToErrorResponse(this ErrorList errors)
    {
        var envelope = Envelop.Error(errors);

        var statusCode = GetErrorListStatusCode(errors);

        return new ObjectResult(envelope) { StatusCode = statusCode };
    }

    // public static ActionResult ToResponse(this ValidationResult validationResult)
    // {
    //     if (validationResult.IsValid)
    //     {
    //         throw new InvalidOperationException("Result cannot be successfully validated.");
    //     }
    //
    //     var validationErrors = validationResult.Errors;
    //
    //     var errors = validationErrors.Select(v => Error.Serialize(v.ErrorMessage));
    //
    //     var envelope = Envelop.Error(new ErrorList(errors));
    //
    //     return new ObjectResult(envelope) { StatusCode = StatusCodes.Status400BadRequest };
    // }

    private static int GetErrorListStatusCode(ErrorList error)
    {
        var errorTypes = error.Errors.GroupBy(e => e.Type).Select(g => g.Key).ToArray();

        if (errorTypes.Contains(ErrorType.Failure))
        {
            return StatusCodes.Status500InternalServerError;
        }

        if (errorTypes.Contains(ErrorType.Conflict))
        {
            return StatusCodes.Status409Conflict;
        }

        if (errorTypes.Contains(ErrorType.NotFound))
        {
            return StatusCodes.Status404NotFound;
        }

        if (errorTypes.Contains(ErrorType.Validation))
        {
            return StatusCodes.Status400BadRequest;
        }

        return StatusCodes.Status500InternalServerError;
    }

    private static int GetStatusCode(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
        return statusCode;
    }
}
