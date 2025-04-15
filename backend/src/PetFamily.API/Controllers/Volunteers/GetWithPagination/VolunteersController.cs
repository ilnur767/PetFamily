using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;
using PetFamily.Application.Volunteers.Queries;

namespace PetFamily.API.Controllers.Volunteers.GetWithPagination;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Получение списка волонтеров постранично.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetWithPagination(
        [FromQuery] GetVolunteersWithPaginationRequest request,
        [FromServices] IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery> volunteersWithPaginationHandler,
        CancellationToken cancellationToken)
    {
        var command = new GetVolunteersWithPaginationQuery(request.Page, request.PageSize);

        var result = await volunteersWithPaginationHandler.Handle(command, cancellationToken);

        return Ok(Envelop.Ok(result));
    }
}
