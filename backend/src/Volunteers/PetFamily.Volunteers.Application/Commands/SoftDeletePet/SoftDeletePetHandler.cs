using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Volunteers.Application.Commands.SoftDeletePet;

[UsedImplicitly]
public sealed class SoftDeletePetHandler : ICommandHandler<SoftDeletePetCommand>
{
    private readonly ILogger<SoftDeletePetHandler> _logger;
    private readonly TimeProvider _timeProvider;
    private readonly IVolunteersRepository _volunteersRepository;

    public SoftDeletePetHandler(IVolunteersRepository volunteersRepository, TimeProvider timeProvider, ILogger<SoftDeletePetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(SoftDeletePetCommand command, CancellationToken cancellationToken)
    {
        var volunteer = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return volunteer.Error.ToErrorList();
        }

        var deleteResult = volunteer.Value.PetSoftDelete(command.PetId, _timeProvider.GetUtcNow().UtcDateTime);

        if (deleteResult.IsFailure)
        {
            return deleteResult.Error.ToErrorList();
        }

        await _volunteersRepository.Save(volunteer.Value, cancellationToken);

        _logger.LogInformation("Pet with id '{id}' was deleted", command.PetId);

        return Result.Success<ErrorList>();
    }
}

public record SoftDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
