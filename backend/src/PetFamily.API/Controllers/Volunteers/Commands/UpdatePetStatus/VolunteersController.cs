using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.UpdatePetStatus;

namespace PetFamily.API.Controllers.Volunteers.Commands.UpdatePetStatus;

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
