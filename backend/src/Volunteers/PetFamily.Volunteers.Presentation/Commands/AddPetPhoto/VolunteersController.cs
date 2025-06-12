using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Commands.AddPetPhoto;
using static PetFamily.Framework.ResponseExtensions;

namespace PetFamily.Volunteers.Presentation.Commands.AddPetPhoto;

[Authorize]
[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Добавление фотографий питомца.
    /// </summary>
    [HttpPost]
    [Route("{id:guid}/{petId:guid}/pets/photo")]
    public async Task<ActionResult<string>> AddPetPhotos(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] ICommandHandler<IReadOnlyCollection<string>, AddPetPhotoCommand> addPetPhotoCommandHandler,
        CancellationToken cancellationToken)
    {
        List<UploadPhotoDto> filesDto = [];
        try
        {
            foreach (var file in files)
            {
                var stream = file.OpenReadStream();
                filesDto.Add(new UploadPhotoDto(stream, file.FileName));
            }

            var command = new AddPetPhotoCommand(id, petId, filesDto);

            var result = await addPetPhotoCommandHandler.Handle(command, cancellationToken);
            if (result.IsFailure)
            {
                return result.Error.ToErrorResponse();
            }

            return Ok(Envelop.Ok(result.Value));
        }
        finally
        {
            foreach (var file in filesDto)
            {
                await file.Content.DisposeAsync();
            }
        }
    }
}
