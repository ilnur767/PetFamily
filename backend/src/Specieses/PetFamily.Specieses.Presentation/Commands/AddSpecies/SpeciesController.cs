using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Specieses.Application.Commands.AddSpecies;
using PetFamily.Specieses.Contracts.Requests;

namespace PetFamily.Specieses.Presentation.Commands.AddSpecies;

[Authorize]
[ApiController]
[Route("[controller]")]
[Permission(PermissionTypes.Species.Delete)]
public class SpeciesController : ControllerBase
{
    /// <summary>
    ///     Создание вида животного.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<string>> CreateSpecies(
        [FromServices] ICommandHandler<Guid, CreateSpeciesCommand> commandHandler,
        [FromBody] CreateSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        var result = await commandHandler.Handle(new CreateSpeciesCommand(request.Name), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}
