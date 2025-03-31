using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Volunteers.Restore;

public class RestoreVolunteerHandler
{
    private readonly ILogger<RestoreVolunteerHandler> _logger;
    private readonly IValidator<RestoreVolunteerCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public RestoreVolunteerHandler(IVolunteersRepository volunteersRepository,
        ILogger<RestoreVolunteerHandler> logger, IValidator<RestoreVolunteerCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(RestoreVolunteerCommand restoreVolunteerCommand,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(restoreVolunteerCommand, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteerResult = await _volunteersRepository.GetById(restoreVolunteerCommand.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error.ToErrorList();
        }

        var volunteer = volunteerResult.Value;

        volunteer.Restore();

        await _volunteersRepository.Save(volunteer, cancellationToken);

        _logger.LogInformation("Volunteer with id {volunteerId} has been restored.", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}

public record RestoreVolunteerCommand(Guid VolunteerId);
