using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;
using PetFamily.Files.Application.UploadFile;
using PetFamily.Framework;

namespace PetFamily.Files.Presentation.Upload;

[Authorize]
[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<string>> UploadFiles(IFormFile file,
        [FromServices] UploadFileHandler uploadFileHandler,
        [FromQuery] string bucketName,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        var command = new AddFileCommand(stream, file.FileName, bucketName);

        var result = await uploadFileHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}
