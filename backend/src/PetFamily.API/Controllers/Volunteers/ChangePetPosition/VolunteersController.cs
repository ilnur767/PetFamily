using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.ChangePetPosition;

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
        [FromServices] IValidator<ChangePetPositionCommand> validator,
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromServices] ChangePetPositionHandler commandHandler,
        [FromBody] ChangePetPositionRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id, petId);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToResponse();
        }

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
