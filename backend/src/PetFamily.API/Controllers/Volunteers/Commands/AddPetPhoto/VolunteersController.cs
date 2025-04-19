using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Volunteers.Commands.AddPetPhoto;
using static PetFamily.API.Extensions.ResponseExtensions;

namespace PetFamily.API.Controllers.Volunteers.Commands.AddPetPhoto;

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
