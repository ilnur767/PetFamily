﻿using CSharpFunctionalExtensions;
using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.DeletePetPhoto;

[UsedImplicitly]
public sealed class DeletePetPhotoCommandHandler : ICommandHandler<DeletePetPhotoCommand>
{
    private const string PhotosBucketName = "photo";
    private readonly IFileProvider _fileProvider;
    private readonly IValidator<DeletePetPhotoCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public DeletePetPhotoCommandHandler(
        IFileProvider fileProvider,
        IVolunteersRepository volunteersRepository,
        IValidator<DeletePetPhotoCommand> validator)
    {
        _fileProvider = fileProvider;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(DeletePetPhotoCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteer = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteer.IsFailure)
        {
            return volunteer.Error.ToErrorList();
        }

        var pet = volunteer.Value.GetPetById(command.PetId);

        if (pet.IsFailure)
        {
            return pet.Error.ToErrorList();
        }

        if (pet.Value.Photos != null)
        {
            var photosList = pet.Value.Photos.Select(p => Photo.Create(p.FileName, p.FilePath).Value);

            var newPhotos = photosList.Where(p => !command.FilesPath.Contains(p.FilePath));

            pet.Value.UpdatePhotos(newPhotos);

            await _volunteersRepository.Save(volunteer.Value, cancellationToken);

            await _fileProvider.DeleteFiles(command.FilesPath, PhotosBucketName, cancellationToken);
        }

        return new UnitResult<ErrorList>();
    }
}
