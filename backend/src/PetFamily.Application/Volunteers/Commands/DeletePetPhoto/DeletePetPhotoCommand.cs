using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Commands.DeletePetPhoto;

public record DeletePetPhotoCommand(Guid VolunteerId, Guid PetId, IEnumerable<string> FilesPath) : ICommand;
