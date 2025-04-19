using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.AddPetPhoto;

public record AddPetPhotoCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadPhotoDto> Photos) : ICommand;
