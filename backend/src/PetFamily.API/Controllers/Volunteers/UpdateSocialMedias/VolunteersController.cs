using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.UpdateSocialMedias;

namespace PetFamily.API.Controllers.Volunteers.UpdateSocialMedias;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Обновление социальных сетей волонтера.
    /// </summary>
    /// <param name="id">Идентификатор волонтера.</param>
    /// <param name="updateSocialMediasHandler">Хендлер для обновления социальных сетей.</param>
    /// <param name="updateSocialMediasDto">Тело запроса.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [HttpPut("{id:guid}/social-medias")]
    public async Task<ActionResult<Guid>> UpdateSocialMedias(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<Guid, UpdateSocialMediasCommand> updateSocialMediasHandler,
        [FromBody] IEnumerable<UpdateSocialMediasDto> updateSocialMediasDto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateSocialMediasCommand(id, updateSocialMediasDto);

        var result = await updateSocialMediasHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}
