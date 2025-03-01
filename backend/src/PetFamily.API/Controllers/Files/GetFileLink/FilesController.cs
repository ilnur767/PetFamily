using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Files.GetFileLink;

namespace PetFamily.API.Controllers.Files.GetFileLink;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<string>> GetFileLink(
        [FromServices] GetFileLinkHandler getFileLinkHandlerHandler,
        [FromQuery] string fileName,
        [FromQuery] string bucketName,
        CancellationToken cancellationToken)
    {
        var command = new GetFileLinkCommand(fileName, bucketName);
        var result = await getFileLinkHandlerHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}