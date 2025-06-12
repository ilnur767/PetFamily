using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;
using PetFamily.Volunteers.Application.Commands.SoftDeletePet;

namespace PetFamily.Volunteers.Presentation.Commands.PetSoftDelete;

[Authorize]
[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Удаление питомца.
    /// </summary>
    [HttpDelete]
    [Route("{id:guid}/pet/{petId:guid}/soft")]
    public async Task<ActionResult<string>> PetSoftDelete(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromServices] ICommandHandler<SoftDeletePetCommand> commandHandler,
        CancellationToken cancellationToken)
    {
        var result = await commandHandler.Handle(new SoftDeletePetCommand(id, petId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return NoContent();
    }
}
