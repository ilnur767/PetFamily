using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Volunteers.SoftDelete;

public sealed class SoftDeleteVolunteerHandler
{
    private readonly ILogger<SoftDeleteVolunteerHandler> _logger;
    private readonly TimeProvider _timeProvider;
    private readonly IVolunteersRepository _volunteersRepository;

    public SoftDeleteVolunteerHandler(IVolunteersRepository volunteersRepository,
        ILogger<SoftDeleteVolunteerHandler> logger, TimeProvider timeProvider)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _timeProvider = timeProvider;
    }

    public async Task<Result<Guid, Error>> Handle(SoftDeleteVolunteerRequest softDeleteVolunteerRequest,
        CancellationToken cancellationToken)
    {
        var volunteerResult =
            await _volunteersRepository.GetById(softDeleteVolunteerRequest.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error;
        }

        var volunteer = volunteerResult.Value;

        volunteer.SoftDelete(_timeProvider.GetUtcNow().UtcDateTime);
        await _volunteersRepository.Save(volunteer, cancellationToken);
        _logger.LogInformation("Volunteer with id {volunteerId} has been removed.", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}

public record SoftDeleteVolunteerRequest(Guid VolunteerId);