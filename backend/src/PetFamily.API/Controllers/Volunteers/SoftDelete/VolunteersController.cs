using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.SoftDelete;
using static PetFamily.API.Extensions.ResponseExtensions;

namespace PetFamily.API.Controllers.Volunteers.SoftDelete;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Удаление волонтера.
    /// </summary>
    /// <param name="softDeleteVolunteerHandler">Хендлер для удаления волонтера.</param>
    /// <param name="id">Идентификатор волонтера на удаление.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpDelete("{id:guid}/soft")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] ICommandHandler<Guid, SoftDeleteVolunteerCommand> softDeleteVolunteerHandler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await softDeleteVolunteerHandler.Handle(new SoftDeleteVolunteerCommand(id), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}
