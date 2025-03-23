using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Volunteers.DeletePetPhoto;
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
        [FromServices] DeletePetPhotoCommandHandler addPetPhotoCommandHandler,
        [FromServices] IValidator<DeletePetPhotoCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetPhotoCommand(id, petId, request.FilesPath);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToResponse();
        }

        var result = await addPetPhotoCommandHandler.Handle(command, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return NoContent();
    }

    public record DeletePetPhotosRequest(IEnumerable<string> FilesPath);
}
