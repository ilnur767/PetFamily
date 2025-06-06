﻿using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Common;
using PetFamily.Core.Dtos;
using PetFamily.Core.Messaging;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Volunteers.Application.Commands.HardDeletePet;

[UsedImplicitly]
public sealed class HardDeletePetHandler : ICommandHandler<HardDeletePetCommand>
{
    private readonly ILogger<HardDeletePetHandler> _logger;
    private readonly IMessageQueue<IEnumerable<FileInfoDto>> _messageQueue;
    private readonly IVolunteersRepository _volunteersRepository;

    public HardDeletePetHandler(IVolunteersRepository volunteersRepository, IMessageQueue<IEnumerable<FileInfoDto>> messageQueue,
        ILogger<HardDeletePetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _messageQueue = messageQueue;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(HardDeletePetCommand command, CancellationToken cancellationToken)
    {
        var volunteer = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return volunteer.Error.ToErrorList();
        }

        var pet = volunteer.Value.Pets.First(p => p.Id.Value == command.PetId);

        if (pet.Photos.Count > 0)
        {
            var files = pet.Photos.Select(f => new FileInfoDto(f.FilePath, FileProviderConstants.PhotosBucketName));
            await _messageQueue.WriteAsync(files, cancellationToken);
        }

        var deleteResult = volunteer.Value.PetHardDelete(command.PetId);

        if (deleteResult.IsFailure)
        {
            return deleteResult.Error.ToErrorList();
        }

        await _volunteersRepository.Save(volunteer.Value, cancellationToken);

        _logger.LogInformation("Pet with id '{id}' was hard deleted", command.PetId);

        return new UnitResult<ErrorList>();
    }
}

public record HardDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
