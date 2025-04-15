using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.UpdateRequisites;

namespace PetFamily.API.Controllers.Volunteers.UpdateRequisites;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Обновление реквизитов волонтера.
    /// </summary>
    /// <param name="id">Идентификатор волонтера.</param>
    /// <param name="validator">Валидатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <param name="updateRequisitesHandler">Хендлер обновления реквезитов.</param>
    /// <param name="updateRequisitesDto">Тело запроса.</param>
    /// <returns></returns>
    [HttpPut("{id:guid}/requisites")]
    public async Task<ActionResult<Guid>> UpdateRequisites(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<Guid, UpdateRequisitesCommand> updateRequisitesHandler,
        [FromBody] IEnumerable<UpdateRequisiteDto> updateRequisitesDto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateRequisitesCommand(id, updateRequisitesDto);

        var result = await updateRequisitesHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}
