using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.AddPet;

namespace PetFamily.API.Controllers.Volunteers.AddPet;

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
        [FromServices] IValidator<AddPetCommand> validator,
        [FromRoute] Guid id,
        [FromServices] AddPetCommandHandler commandHandler,
        [FromBody] AddPetRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToResponse();
        }

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
