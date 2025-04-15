using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.DeletePetPhoto;
using static PetFamily.API.Extensions.ResponseExtensions;

namespace PetFamily.API.Controllers.Volunteers.DeletePetPhoto;

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
