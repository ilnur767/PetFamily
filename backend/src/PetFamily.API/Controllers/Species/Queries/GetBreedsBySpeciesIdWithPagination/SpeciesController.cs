using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;
using PetFamily.Application.Specieses.Queries;

namespace PetFamily.API.Controllers.Species.Queries.GetBreedsBySpeciesIdWithPagination;

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
        var result = await commandHandler.Handle(request.ToQuery(speciesId), cancellationToken);

        return Ok(Envelop.Ok(result));
    }
}

public record GetBreedsBySpeсiesIdWithPaginationRequest(int Page, int PageSize)
{
    public GetBreedsBySpeiesIdWithPaginationQuery ToQuery(Guid speciesId)
    {
        return new GetBreedsBySpeiesIdWithPaginationQuery(speciesId, Page, PageSize);
    }
}
