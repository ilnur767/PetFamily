using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Volunteers.HardDelete;

public sealed class HardDeleteVolunteerHandler
{
    private readonly ILogger<HardDeleteVolunteerHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;

    public HardDeleteVolunteerHandler(IVolunteersRepository volunteersRepository,
        ILogger<HardDeleteVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(HardDeleteVolunteerRequest softDeleteVolunteerRequest,
        CancellationToken cancellationToken)
    {
        var volunteerResult = await _volunteersRepository.GetById(softDeleteVolunteerRequest.VolunteerId);
        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error;
        }

        var volunteerId = await _volunteersRepository.HardDelete(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Volunteer with id {volunteerId} has been removed.", volunteerId);

        return volunteerId;
    }
}

public record HardDeleteVolunteerRequest(Guid VolunteerId);