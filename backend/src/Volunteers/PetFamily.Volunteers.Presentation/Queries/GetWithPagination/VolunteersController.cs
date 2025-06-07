using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Queries.GetWithPagination;

namespace PetFamily.Volunteers.Presentation.Queries.GetWithPagination;

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
