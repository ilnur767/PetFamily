using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Queries.GetById;

namespace PetFamily.Volunteers.Presentation.Queries.GetById;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Получение волонтера по идентификатору.
    /// </summary>
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<VolunteerDto, GetVolunteerByIdQuery> volunteerByIdHandler,
        CancellationToken cancellationToken)
    {
        var result = await volunteerByIdHandler.Handle(new GetVolunteerByIdQuery(id), cancellationToken);

        return Ok(Envelop.Ok(result));
    }
}
