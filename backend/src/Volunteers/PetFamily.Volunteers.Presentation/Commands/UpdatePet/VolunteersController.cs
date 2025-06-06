using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Volunteers.Application.Commands.UpdatePet;

namespace PetFamily.Volunteers.Presentation.Commands.UpdatePet;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Обновление питомца.
    /// </summary>
    [HttpPut]
    [Route("{id:guid}/pet/{petId:guid}")]
    public async Task<ActionResult<string>> UpdatePet(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromServices] ICommandHandler<Guid, UpdatePetCommand> commandHandler,
        [FromBody] UpdatePetRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id, petId);

        var result = await commandHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(Envelop.Ok(result.Value));
    }
}

public record UpdatePetRequest(
    string NickName,
    string Description,
    string PhoneNumber,
    double Height,
    double Weight,
    bool IsCastrated,
    bool IsVaccinated,
    DateTime DateOfBirth,
    string HealthInformation,
    string Address,
    Guid SpeciesId,
    Guid BreeedId)
{
    public UpdatePetCommand ToCommand(Guid id, Guid petId)
    {
        return new UpdatePetCommand(id, petId, NickName, Description, PhoneNumber, Height, Weight, IsCastrated, IsVaccinated, DateOfBirth, Address,
            HealthInformation,
            SpeciesId, BreeedId);
    }
}
