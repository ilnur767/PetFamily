using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.UpdatePetMainPhoto;

namespace PetFamily.API.Controllers.Volunteers.Commands.UpdatePetMainPhoto;

[ApiController]
[Route("[controller]")]
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
