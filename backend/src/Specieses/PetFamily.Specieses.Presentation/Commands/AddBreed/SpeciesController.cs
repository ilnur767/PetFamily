using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Specieses.Application.Commands.AddBreed;
using PetFamily.Specieses.Contracts.Requests;

namespace PetFamily.Specieses.Presentation.Commands.AddBreed;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SpeciesController : ControllerBase
{
    /// <summary>
    ///     Создание породы животного.
    /// </summary>
    [HttpPost]
    [Route("{speciesId:guid}/breed")]
    public async Task<ActionResult<string>> CreateBreed(
        [FromRoute] Guid speciesId,
        [FromServices] ICommandHandler<Guid, CreateBreedCommand> commandHandler,
        [FromBody] CreateBreedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await commandHandler.Handle(new CreateBreedCommand(speciesId, request.Name), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}
