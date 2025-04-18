using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.Restore;
using static PetFamily.API.Extensions.ResponseExtensions;

namespace PetFamily.API.Controllers.Volunteers.Commands.Restore;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Восстановление волонтера.
    /// </summary>
    /// <param name="restoreVolunteerHandler">Хендлер для восстановления волонтера.</param>
    /// <param name="id">Идентификатор волонтера на восстановление.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPut("{id:guid}/restore")]
    public async Task<ActionResult<Guid>> Restore(
        [FromServices] ICommandHandler<Guid, RestoreVolunteerCommand> restoreVolunteerHandler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await restoreVolunteerHandler.Handle(new RestoreVolunteerCommand(id), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}
