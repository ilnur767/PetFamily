using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.HardDeletePet;

namespace PetFamily.API.Controllers.Volunteers.Commands.PetHardDelete;

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
