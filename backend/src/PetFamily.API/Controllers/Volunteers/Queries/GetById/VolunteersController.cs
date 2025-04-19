using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Queries;

namespace PetFamily.API.Controllers.Volunteers.Queries.GetById;

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
