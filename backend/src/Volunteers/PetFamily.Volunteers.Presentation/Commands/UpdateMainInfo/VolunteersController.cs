using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Volunteers.Application.Commands.UpdateMainInfo;

namespace PetFamily.Volunteers.Presentation.Commands.UpdateMainInfo;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Обновление основной информации волонтера.
    /// </summary>
    /// <param name="id">Идентификатор волонтера.</param>
    /// <param name="updateMainInfoHandler">Хендлер для обновления волонтера.</param>
    /// <param name="request">Тело запроса.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<Guid, UpdateMainInfoCommand> updateMainInfoHandler,
        [FromBody] UpdateMainInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var result = await updateMainInfoHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}

public record UpdateMainInfoRequest(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string PhoneNumber,
    string Description,
    int WorkExperience)
{
    public UpdateMainInfoCommand ToCommand(Guid id)
    {
        return new UpdateMainInfoCommand(id,
            new UpdateMainInfoDto(FirstName, LastName, MiddleName, Email, PhoneNumber, Description, WorkExperience));
    }
}
