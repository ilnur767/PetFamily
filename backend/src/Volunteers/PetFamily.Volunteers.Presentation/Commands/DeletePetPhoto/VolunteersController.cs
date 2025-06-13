using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Commands.DeletePetPhoto;
using static PetFamily.Framework.ResponseExtensions;

namespace PetFamily.Volunteers.Presentation.Commands.DeletePetPhoto;

[Authorize]
[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Удаление фотографий питомца.
    /// </summary>
    [HttpPut]
    [Route("{id:guid}/{petId:guid}/pets/photo")]
    public async Task<ActionResult<string>> DelePetPhoto(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromBody] DeletePetPhotosRequest request,
        [FromServices] ICommandHandler<DeletePetPhotoCommand> addPetPhotoCommandHandler,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetPhotoCommand(id, petId, request.FilesPath);

        var result = await addPetPhotoCommandHandler.Handle(command, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return NoContent();
    }

    public record DeletePetPhotosRequest(IEnumerable<string> FilesPath);
}
