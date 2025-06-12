using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Volunteers.Application.Commands.Create;

namespace PetFamily.Volunteers.Presentation.Commands.Create;

[Authorize]
[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Создание волонтера.
    /// </summary>
    /// <param name="createVolunteerHandler">Хендлер для создания волонтера.</param>
    /// <param name="createVolunteerRequest">Тело запроса.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] ICommandHandler<Guid, CreateVolunteerCommand> createVolunteerHandler,
        [FromBody] CreateVolunteerRequest createVolunteerRequest,
        CancellationToken cancellationToken = default)
    {
        var createCommand = new CreateVolunteerCommand(
            createVolunteerRequest.FirstName,
            createVolunteerRequest.LastName,
            createVolunteerRequest.MiddleName,
            createVolunteerRequest.Email,
            createVolunteerRequest.PhoneNumber,
            createVolunteerRequest.Requisites?.Select(r => new CreateRequisiteCommand(r.Name, r.Description)),
            createVolunteerRequest.SocialMedias?.Select(s => new CreateSocialMediaCommand(s.Name, s.Link))
        );

        var result = await createVolunteerHandler.Handle(createCommand, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}
