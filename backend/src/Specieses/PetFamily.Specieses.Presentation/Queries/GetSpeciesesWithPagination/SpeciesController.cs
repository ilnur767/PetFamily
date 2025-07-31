using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using PetFamily.Framework.Authorization;
using PetFamily.Specieses.Application.Queries;
using PetFamily.Specieses.Contracts.Requests;

namespace PetFamily.Specieses.Presentation.Queries.GetSpeciesesWithPagination;

[Authorize]
[ApiController]
[Route("[controller]")]
[Permission(PermissionTypes.Species.Read)]
public class SpeciesController : ControllerBase
{
    /// <summary>
    ///     Получение всех видов животных постранично.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetSpeciesWithPagination(
        [FromServices] IQueryHandler<PagedList<SpeciesDto>, GetSpeiciesWithPaginationQuery> commandHandler,
        [FromQuery] GetSpeciesesWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await commandHandler.Handle(new GetSpeiciesWithPaginationQuery(request.Page, request.PageSize), cancellationToken);

        return Ok(Envelop.Ok(result));
    }
}
