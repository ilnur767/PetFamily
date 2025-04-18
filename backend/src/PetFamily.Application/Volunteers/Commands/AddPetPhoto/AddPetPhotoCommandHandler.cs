using CSharpFunctionalExtensions;
using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Volunteers.Commands.AddPetPhoto;

[UsedImplicitly]
public class AddPetPhotoCommandHandler : ICommandHandler<IReadOnlyCollection<string>, AddPetPhotoCommand>
{
    private const string PhotosBucketName = "photo";
    private readonly IFileProvider _fileProvider;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly IValidator<AddPetPhotoCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;


    public AddPetPhotoCommandHandler(
        IVolunteersRepository volunteersRepository,
        IFileProvider fileProvider,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IValidator<AddPetPhotoCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _fileProvider = fileProvider;
        _messageQueue = messageQueue;
        _validator = validator;
    }

    public async Task<Result<IReadOnlyCollection<string>, ErrorList>> Handle(AddPetPhotoCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var filesData = new List<FileData>();
        var photos = new List<Photo>();

        foreach (var photo in command.Photos)
        {
            var filePath = $"{Guid.NewGuid()}_{photo.FileName}";
            var fileData = new FileData(photo.Content, new FileInfo(filePath, PhotosBucketName));
            filesData.Add(fileData);

            photos.Add(Photo.Create(photo.FileName, filePath).Value);
        }

        var result = await _fileProvider.UploadFiles(filesData, PhotosBucketName, cancellationToken);

        if (result.IsFailure)
        {
            await _messageQueue.WriteAsync(filesData.Select(f => f.Info), cancellationToken);

            return result.Error.ToErrorList();
        }

        var savePhotoResult = await SaveToDb(command, cancellationToken, photos);

        if (savePhotoResult.IsFailure)
        {
            await _messageQueue.WriteAsync(filesData.Select(f => f.Info), cancellationToken);

            return savePhotoResult.Error.ToErrorList();
        }

        return result.Value.ToList();
    }

    private async Task<UnitResult<Error>> SaveToDb(AddPetPhotoCommand command, CancellationToken cancellationToken, List<Photo> photos)
    {
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

        pet.Value.UpdatePhotos(photos);

        await _volunteersRepository.Save(volunteer.Value, cancellationToken);

        return Result.Success<Error>();
    }
}
