using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Common;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.UpdatePetMainPhoto;

[UsedImplicitly]
public sealed class UpdatePetMainPhotoHandler : ICommandHandler<UpdatePetMainPhotoCommand>
{
    private readonly ILogger<UpdatePetMainPhotoHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;

    public UpdatePetMainPhotoHandler(IVolunteersRepository volunteersRepository, ILogger<UpdatePetMainPhotoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(UpdatePetMainPhotoCommand command, CancellationToken cancellationToken)
    {
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

        var photoResult = Photo.Create(command.FileName, command.FilePath, true);
        if (photoResult.IsFailure)
        {
            return photoResult.Error.ToErrorList();
        }

        var updateMainPhotoResult = pet.Value.UpdateMainPhoto(photoResult.Value);

        if (updateMainPhotoResult.IsFailure)
        {
            return updateMainPhotoResult.Error.ToErrorList();
        }

        await _volunteersRepository.Save(volunteer.Value, cancellationToken);

        _logger.LogInformation("Main photo updated for pet with id: {id}", command.PetId);

        return Result.Success<ErrorList>();
    }
}

public record UpdatePetMainPhotoCommand(Guid VolunteerId, Guid PetId, string FilePath, string FileName) : ICommand;
