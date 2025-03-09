using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Files.AddFile;

namespace PetFamily.API.Controllers.Files.Upload;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<string>> UploadFiles(IFormFile file,
        [FromServices] AddFileHandler addFileHandler,
        [FromQuery] string bucketName,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        var command = new AddFileCommand(stream, file.FileName, bucketName);

        var result = await addFileHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}