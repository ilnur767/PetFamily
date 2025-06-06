namespace PetFamily.Volunteers.Application.Commands.AddPetPhoto;

public record UploadPhotoDto(Stream Content, string FileName);
