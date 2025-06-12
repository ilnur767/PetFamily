using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;
using PetFamily.Files.Application.GetFileLink;
using PetFamily.Framework;

namespace PetFamily.Files.Presentation.GetFileLink;

[Authorize]
[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
    /// <summary>
    ///     Получить ссылку на файл.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<string>> GetFileLink(
        [FromServices] GetFileLinkHandler getFileLinkHandler,
        [FromQuery] string fileName,
        [FromQuery] string bucketName,
        CancellationToken cancellationToken)
    {
        var command = new GetFileLinkCommand(fileName, bucketName);
        var result = await getFileLinkHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}
