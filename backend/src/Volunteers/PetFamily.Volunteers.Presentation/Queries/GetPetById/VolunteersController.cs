using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Queries.GetPetById;

namespace PetFamily.Volunteers.Presentation.Queries.GetPetById;

[Authorize]
[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Получение питомца по идентификатору.
    /// </summary>
    [HttpGet]
    [Route("pet/{id:guid}")]
    public async Task<IActionResult> GetPetById(
        [FromServices] IQueryHandler<PetDto?, GetPetByIdQuery> petsWithPaginationHandler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await petsWithPaginationHandler.Handle(new GetPetByIdQuery(id), cancellationToken);

        return Ok(Envelop.Ok(result));
    }
}
