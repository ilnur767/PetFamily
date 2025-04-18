using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Specieses.Commands.AddSpecies;

namespace PetFamily.API.Controllers.Species.Commands.AddSpecies;

[ApiController]
[Route("[controller]")]
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
        var command = request.ToCommand();

        var result = await commandHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand()
    {
        return new CreateSpeciesCommand(Name);
    }
}
