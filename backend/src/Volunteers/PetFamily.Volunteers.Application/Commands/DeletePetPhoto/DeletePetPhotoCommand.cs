using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Commands.DeletePetPhoto;

public record DeletePetPhotoCommand(Guid VolunteerId, Guid PetId, IEnumerable<string> FilesPath) : ICommand;
