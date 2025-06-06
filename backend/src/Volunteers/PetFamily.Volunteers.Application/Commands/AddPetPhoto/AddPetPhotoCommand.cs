using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Commands.AddPetPhoto;

public record AddPetPhotoCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadPhotoDto> Photos) : ICommand;
