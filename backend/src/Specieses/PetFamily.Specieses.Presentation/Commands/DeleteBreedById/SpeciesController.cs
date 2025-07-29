using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Specieses.Application.Commands.DeleteBreedById;

namespace PetFamily.Specieses.Presentation.Commands.DeleteBreedById;

[Authorize]
[ApiController]
[Route("[controller]")]
[Permission(PermissionTypes.Species.Create)]
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
