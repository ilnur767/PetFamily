using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Volunteers.Application.Commands.UpdatePetMainPhoto;

namespace PetFamily.Volunteers.Presentation.Commands.UpdatePetMainPhoto;

[Authorize]
[ApiController]
[Route("[controller]")]
[Permission(PermissionTypes.Pet.Update)]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Обновление главной фотографии питомца.
    /// </summary>
    [HttpPut]
    [Route("{id:guid}/pet/{petId:guid}/main-photo")]
    public async Task<ActionResult<string>> UpdatePetMainPhoto(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromServices] ICommandHandler<UpdatePetMainPhotoCommand> commandHandler,
        [FromBody] UpdatePetMainPhotoRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id, petId);

        var result = await commandHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return NoContent();
    }
}

public record UpdatePetMainPhotoRequest(
    string FilePath,
    string FileName)
{
    public UpdatePetMainPhotoCommand ToCommand(Guid id, Guid petId)
    {
        return new UpdatePetMainPhotoCommand(id, petId, FilePath, FileName);
    }
}
