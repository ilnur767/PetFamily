using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Specieses.Application.Commands.DeleteSpeciesById;

namespace PetFamily.Specieses.Presentation.Commands.DeleteSpeciesById;

[Authorize]
[ApiController]
[Route("[controller]")]
[Permission(PermissionTypes.Species.Delete)]
public class SpeciesController : ControllerBase
{
    /// <summary>
    ///     Удаление вида животного.
    /// </summary>
    [HttpDelete("{speciesId:guid}")]
    public async Task<ActionResult<string>> DeleteSpecies(
        [FromServices] ICommandHandler<DeleteSpeciesByIdCommand> commandHandler,
        [FromRoute] Guid speciesId,
        CancellationToken cancellationToken)
    {
        var result = await commandHandler.Handle(new DeleteSpeciesByIdCommand(speciesId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return NoContent();
    }
}
