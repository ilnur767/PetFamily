using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Specieses.Commands.AddBreed;

namespace PetFamily.API.Controllers.Species.Commands.AddBreed;

[ApiController]
[Route("[controller]")]
public class SpeciesController : ControllerBase
{
    /// <summary>
    ///     Создание породы животного.
    /// </summary>
    [HttpPost]
    [Route("{speciesId:guid}/breed")]
    public async Task<ActionResult<string>> CreateSpecies(
        [FromRoute] Guid speciesId,
        [FromServices] ICommandHandler<Guid, CreateBreedCommand> commandHandler,
        [FromBody] CreateBreedRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(speciesId);

        var result = await commandHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}

public record CreateBreedRequest(string Name)
{
    public CreateBreedCommand ToCommand(Guid speciesId)
    {
        return new CreateBreedCommand(speciesId, Name);
    }
}
