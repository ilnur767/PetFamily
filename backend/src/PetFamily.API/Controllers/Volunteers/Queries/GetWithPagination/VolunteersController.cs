using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;
using PetFamily.Application.Volunteers.Queries.GetWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Queries.GetWithPagination;

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
        var query = new GetVolunteersWithPaginationQuery(request.Page, request.PageSize);

        var result = await volunteersWithPaginationHandler.Handle(query, cancellationToken);

        return Ok(Envelop.Ok(result));
    }
}
