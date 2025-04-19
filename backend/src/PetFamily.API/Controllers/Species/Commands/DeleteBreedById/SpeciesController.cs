using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Specieses.Commands.DeleteBreedById;

namespace PetFamily.API.Controllers.Species.Commands.DeleteBreedById;

[ApiController]
[Route("[controller]")]
public class SpeciesController : ControllerBase
{
    /// <summary>
    ///     Удаление породы животного.
    /// </summary>
    [HttpDelete("{speciesId:guid}/breed/{breedId:guid}")]
    public async Task<ActionResult<string>> DeleteSpecies(
        [FromServices] ICommandHandler<DeleteBreedByIdCommand> commandHandler,
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,
        CancellationToken cancellationToken)
    {
        var result = await commandHandler.Handle(new DeleteBreedByIdCommand(speciesId, breedId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return NoContent();
    }
}
