using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.AddPetPhoto;

public class AddPetPhotoCommandHandler
{
    private const string PhotosBucketName = "photo";
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteersRepository _volunteersRepository;

    public AddPetPhotoCommandHandler(IVolunteersRepository volunteersRepository, IFileProvider fileProvider)
    {
        _volunteersRepository = volunteersRepository;
        _fileProvider = fileProvider;
    }

    public async Task<Result<IReadOnlyCollection<string>, Error>> Handle(AddPetPhotoCommand command, CancellationToken cancellationToken)
    {
        var filesData = new List<FileData>();
        var photos = new List<Photo>();

        foreach (var photo in command.Photos)
        {
            var filePath = $"{Guid.NewGuid()}_{photo.FileName}";
            var fileData = new FileData(photo.Content, filePath);
            filesData.Add(fileData);

            photos.Add(Photo.Create(photo.FileName, filePath).Value);
        }

        var result = await _fileProvider.UploadFiles(filesData, PhotosBucketName, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var volunteer = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteer.IsFailure)
        {
            return volunteer.Error;
        }

        var pet = volunteer.Value.GetPetById(command.PetId);

        if (pet.IsFailure)
        {
            return pet.Error;
        }

        var photosList = PetPhotosList.Create(photos);

        pet.Value.AddPhotos(photosList.Value);

        await _volunteersRepository.Save(volunteer.Value, cancellationToken);


        return result;
    }
}
