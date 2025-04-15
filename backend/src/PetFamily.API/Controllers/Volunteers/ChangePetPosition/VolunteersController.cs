using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.ChangePetPosition;

namespace PetFamily.API.Controllers.Volunteers.ChangePetPosition;

[ApiController]
[Route("[controller]")]
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
