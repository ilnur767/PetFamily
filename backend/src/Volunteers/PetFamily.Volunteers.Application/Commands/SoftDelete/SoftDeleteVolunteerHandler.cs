using CSharpFunctionalExtensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Volunteers.Application.Commands.SoftDelete;

[UsedImplicitly]
public sealed class SoftDeleteVolunteerHandler : ICommandHandler<Guid, SoftDeleteVolunteerCommand>
{
    private readonly ILogger<SoftDeleteVolunteerHandler> _logger;
    private readonly TimeProvider _timeProvider;
    private readonly IValidator<SoftDeleteVolunteerCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public SoftDeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<SoftDeleteVolunteerHandler> logger,
        TimeProvider timeProvider,
        IValidator<SoftDeleteVolunteerCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _timeProvider = timeProvider;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(SoftDeleteVolunteerCommand softDeleteVolunteerCommand,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(softDeleteVolunteerCommand, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteerResult =
            await _volunteersRepository.GetById(softDeleteVolunteerCommand.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error.ToErrorList();
        }

        var volunteer = volunteerResult.Value;

        volunteer.SoftDelete(_timeProvider.GetUtcNow().UtcDateTime);
        await _volunteersRepository.Save(volunteer, cancellationToken);
        _logger.LogInformation("Volunteer with id {volunteerId} has been removed.", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}

public record SoftDeleteVolunteerCommand(Guid VolunteerId) : ICommand;
