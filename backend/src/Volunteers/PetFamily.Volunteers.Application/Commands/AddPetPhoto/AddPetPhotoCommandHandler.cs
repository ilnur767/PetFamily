using CSharpFunctionalExtensions;
using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Common;
using PetFamily.Core.Dtos;
using PetFamily.Core.Extensions;
using PetFamily.Core.Messaging;
using PetFamily.Files.Contracts;
using PetFamily.Files.Contracts.Requests;
using PetFamily.SharedKernel.Common;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.AddPetPhoto;

[UsedImplicitly]
public class AddPetPhotoCommandHandler : ICommandHandler<IReadOnlyCollection<string>, AddPetPhotoCommand>
{
    private readonly IFilesContract _filesContract;
    private readonly IMessageQueue<IEnumerable<FileInfoDto>> _messageQueue;
    private readonly IValidator<AddPetPhotoCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;


    public AddPetPhotoCommandHandler(
        IVolunteersRepository volunteersRepository,
        IFilesContract filesContract,
        IMessageQueue<IEnumerable<FileInfoDto>> messageQueue,
        IValidator<AddPetPhotoCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _filesContract = filesContract;
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

        var filesData = new List<FileDataDto>();
        var photos = new List<Photo>();

        foreach (var photo in command.Photos)
        {
            var filePath = $"{Guid.NewGuid()}_{photo.FileName}";
            var fileData = new FileDataDto(photo.Content, new FileInfoDto(filePath, FileProviderConstants.PhotosBucketName));
            filesData.Add(fileData);

            photos.Add(Photo.Create(photo.FileName, filePath).Value);
        }

        var result = await _filesContract.UploadFiles(new UploadFilesRequest(filesData, FileProviderConstants.PhotosBucketName), cancellationToken);

        if (result.IsFailure)
        {
            await _messageQueue.WriteAsync(filesData.Select(f => f.Info), cancellationToken);

            return result.Error;
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
