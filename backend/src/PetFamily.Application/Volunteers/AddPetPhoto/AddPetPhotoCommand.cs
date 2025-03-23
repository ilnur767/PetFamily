namespace PetFamily.Application.Volunteers.AddPetPhoto;

public record AddPetPhotoCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadPhotoDto> Photos);
