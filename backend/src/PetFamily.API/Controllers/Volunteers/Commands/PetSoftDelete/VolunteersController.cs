using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.SoftDeletePet;

namespace PetFamily.API.Controllers.Volunteers.Commands.PetSoftDelete;

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
