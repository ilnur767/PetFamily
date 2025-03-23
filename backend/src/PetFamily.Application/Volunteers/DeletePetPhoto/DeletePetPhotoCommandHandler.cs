// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.DeletePetPhoto;

public sealed class DeletePetPhotoCommandHandler
{
    private const string PhotosBucketName = "photo";
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteersRepository _volunteersRepository;

    public DeletePetPhotoCommandHandler(IFileProvider fileProvider, IVolunteersRepository volunteersRepository)
    {
        _fileProvider = fileProvider;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<UnitResult<Error>> Handle(DeletePetPhotoCommand command, CancellationToken cancellationToken)
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

        if (pet.Value.PhotosList != null)
        {
            var photosList = pet.Value.PhotosList.Photos.Select(p => Photo.Create(p.FileName, p.FilePath).Value);

            var newPhotos = photosList.Where(p => !command.FilesPath.Contains(p.FilePath));

            pet.Value.AddPhotos(PetPhotosList.Create(newPhotos).Value);

            await _volunteersRepository.Save(volunteer.Value, cancellationToken);

            await _fileProvider.DeleteFiles(command.FilesPath, PhotosBucketName, cancellationToken);
        }

        return new UnitResult<Error>();
    }
}
