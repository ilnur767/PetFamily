using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Commands.HardDelete;
using static PetFamily.Framework.ResponseExtensions;

namespace PetFamily.Volunteers.Presentation.Commands.HardDelete;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Полное удаление волонтера.
    /// </summary>
    /// <param name="hardDeleteVolunteerHandler">Хендлер для полного удаления волонтера.</param>
    /// <param name="id">Идентификатор волонтера на удаление.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpDelete("{id:guid}/hard")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] ICommandHandler<Guid, HardDeleteVolunteerCommand> hardDeleteVolunteerHandler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await hardDeleteVolunteerHandler.Handle(new HardDeleteVolunteerCommand(id), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}
