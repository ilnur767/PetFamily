using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework.Authorization;
using PetFamily.Volunteers.Application.Commands.Restore;
using static PetFamily.Framework.ResponseExtensions;

namespace PetFamily.Volunteers.Presentation.Commands.Restore;

[Authorize]
[ApiController]
[Route("[controller]")]
[Permission(PermissionTypes.Volunteers.Delete)]
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
