using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Volunteers.Application.Commands.ChangePetPosition;

namespace PetFamily.Volunteers.Presentation.Commands.ChangePetPosition;

[Authorize]
[ApiController]
[Route("[controller]")]
[Permission(PermissionTypes.Pet.Update)]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Изменение позиции(порядкового номера) питомца.
    /// </summary>
    [HttpPut]
    [Route("{id:guid}/{petId:guid}/position")]
    public async Task<ActionResult<string>> ChangePetPosition(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromServices] ICommandHandler<ChangePetPositionCommand> commandHandler,
        [FromBody] ChangePetPositionRequest request,
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

public record ChangePetPositionRequest(int NewPosition)
{
    public ChangePetPositionCommand ToCommand(Guid id, Guid petId)
    {
        return new ChangePetPositionCommand(id, petId, NewPosition);
    }
}
