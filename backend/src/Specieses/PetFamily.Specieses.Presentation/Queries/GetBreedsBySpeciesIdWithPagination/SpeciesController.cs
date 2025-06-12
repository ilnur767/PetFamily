using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using PetFamily.Specieses.Application.Queries;
using PetFamily.Specieses.Contracts.Requests;

namespace PetFamily.Specieses.Presentation.Queries.GetBreedsBySpeciesIdWithPagination;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SpeciesController : ControllerBase
{
    /// <summary>
    ///     Получение всех пород животных по виду постранично.
    /// </summary>
    [HttpGet("{speciesId:guid}/breeds")]
    public async Task<IActionResult> GetBreedsBySpeciesIdWithPagination(
        [FromServices] IQueryHandler<PagedList<BreedDto>, GetBreedsBySpeiesIdWithPaginationQuery> commandHandler,
        [FromQuery] GetBreedsBySpeсiesIdWithPaginationRequest request,
        [FromRoute] Guid speciesId,
        CancellationToken cancellationToken)
    {
        var result = await commandHandler.Handle(new GetBreedsBySpeiesIdWithPaginationQuery(speciesId, request.Page, request.PageSize), cancellationToken);

        return Ok(Envelop.Ok(result));
    }
}
