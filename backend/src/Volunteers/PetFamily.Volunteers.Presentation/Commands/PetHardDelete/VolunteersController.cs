using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;
using PetFamily.Volunteers.Application.Commands.HardDeletePet;

namespace PetFamily.Volunteers.Presentation.Commands.PetHardDelete;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Полное удаление питомца.
    /// </summary>
    [HttpDelete]
    [Route("{id:guid}/pet/{petId:guid}/hard")]
    public async Task<ActionResult<string>> PetSoftDelete(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromServices] ICommandHandler<HardDeletePetCommand> commandHandler,
        CancellationToken cancellationToken)
    {
        var result = await commandHandler.Handle(new HardDeletePetCommand(id, petId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return NoContent();
    }
}
