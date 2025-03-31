using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Volunteers.HardDelete;

public sealed class HardDeleteVolunteerHandler
{
    private readonly ILogger<HardDeleteVolunteerHandler> _logger;
    private readonly IValidator<HardDeleteVolunteerCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public HardDeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<HardDeleteVolunteerHandler> logger,
        IValidator<HardDeleteVolunteerCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(HardDeleteVolunteerCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error.ToErrorList();
        }

        var volunteerId = await _volunteersRepository.HardDelete(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Volunteer with id {volunteerId} has been removed.", volunteerId);

        return volunteerId;
    }
}

public record HardDeleteVolunteerCommand(Guid VolunteerId);
