using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;
using PetFamily.Volunteers.Application.Commands.UpdatePetStatus;

namespace PetFamily.Volunteers.Presentation.Commands.UpdatePetStatus;

[Authorize]
[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Обновление статуса питомца.
    /// </summary>
    [HttpPut]
    [Route("{id:guid}/pet/{petId:guid}/status")]
    public async Task<ActionResult<string>> UpdatePetStatus(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromServices] ICommandHandler<UpdatePetStatusCommand> commandHandler,
        [FromBody] UpdatePetStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await commandHandler.Handle(request.ToCommand(id, petId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return NoContent();
    }
}

public record UpdatePetStatusRequest(string PetStatus)
{
    public UpdatePetStatusCommand ToCommand(Guid id, Guid petId)
    {
        return new UpdatePetStatusCommand(id, petId, PetStatus);
    }
}
