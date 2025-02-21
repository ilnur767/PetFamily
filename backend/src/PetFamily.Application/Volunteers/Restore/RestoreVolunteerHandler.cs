using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Volunteers.Restore;

public class RestoreVolunteerHandler
{
    private readonly ILogger<RestoreVolunteerHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;

    public RestoreVolunteerHandler(IVolunteersRepository volunteersRepository,
        ILogger<RestoreVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(RestoreVolunteerRequest softDeleteVolunteerRequest,
        CancellationToken cancellationToken)
    {
        var volunteerResult = await _volunteersRepository.GetById(softDeleteVolunteerRequest.VolunteerId);
        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error;
        }

        var volunteer = volunteerResult.Value;

        volunteer.Restore();

        await _volunteersRepository.Save(volunteer, cancellationToken);

        _logger.LogInformation("Volunteer with id {volunteerId} has been restored.", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}

public record RestoreVolunteerRequest(Guid VolunteerId);