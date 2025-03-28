﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.UpdateSocialMedias;

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
    /// <param name="validator">Валидатор.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [HttpPut("{id:guid}/social-medias")]
    public async Task<ActionResult<Guid>> UpdateSocialMedias(
        [FromRoute] Guid id,
        [FromServices] UpdateSocialMediasHandler updateSocialMediasHandler,
        [FromBody] IEnumerable<UpdateSocialMediasDto> updateSocialMediasDto,
        [FromServices] IValidator<UpdateSocialMediasCommand> validator,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateSocialMediasCommand(id, updateSocialMediasDto);
        var validation = await validator.ValidateAsync(command, cancellationToken);
        if (validation.IsValid == false)
        {
            return validation.ToResponse();
        }

        var result = await updateSocialMediasHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}