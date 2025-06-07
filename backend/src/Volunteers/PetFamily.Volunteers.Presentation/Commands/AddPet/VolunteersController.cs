using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Volunteers.Application.Commands.AddPet;

namespace PetFamily.Volunteers.Presentation.Commands.AddPet;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Добавление питомца.
    /// </summary>
    [HttpPost]
    [Route("{id:guid}/pets")]
    public async Task<ActionResult<string>> AddPet(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<Guid, AddPetCommand> commandHandler,
        [FromBody] AddPetRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);

        var result = await commandHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}

public record AddPetRequest(
    string NickName,
    string Description,
    string PhoneNumber,
    string PetStatus,
    Guid SpeciesId,
    Guid BreeedId)
{
    public AddPetCommand ToCommand(Guid id)
    {
        return new AddPetCommand(id, NickName, Description, PhoneNumber, PetStatus, SpeciesId, BreeedId);
    }
}
