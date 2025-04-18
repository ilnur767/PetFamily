using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;
using PetFamily.Application.Specieses.Queries;

namespace PetFamily.API.Controllers.Species.Queries.GetSpeciesesWithPagination;

[ApiController]
[Route("[controller]")]
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

public record GetSpeciesesWithPaginationRequest(int Page, int PageSize);
