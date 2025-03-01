﻿using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Files.DeleteFile;

namespace PetFamily.API.Controllers.Files.Delete;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
    [HttpDelete]
    public async Task<ActionResult<string>> Delete(
        [FromServices] DeleteFileHandler deleteFileHandler,
        [FromQuery] string fileName,
        [FromQuery] string bucketName,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFileCommand(fileName, bucketName);
        var result = await deleteFileHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return NoContent();
    }
}