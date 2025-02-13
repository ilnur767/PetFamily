using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public sealed class UpdateRequisitesHandler
{
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;

    public UpdateRequisitesHandler(ILogger<UpdateMainInfoHandler> logger, IVolunteersRepository volunteersRepository)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, Error>> Handle(UpdateRequisitesCommand command, CancellationToken cancellationToken)
    {
        var volunteerResult = await _volunteersRepository.GetById(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error;
        }

        var volunteer = volunteerResult.Value;
        volunteer.UpdateRequisites(
            command.UpdateRequisitesDto.Select(r => Requisite.Create(r.Name, r.Description).Value)
            .ToList());
        
        await _volunteersRepository.Save(volunteer, cancellationToken);

        _logger.LogInformation("Updated requisites for volunteer with id: {volunteerId}", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}

public record UpdateRequisitesCommand(Guid Id, IEnumerable<UpdateRequisiteDto> UpdateRequisitesDto);

public record UpdateRequisiteDto(string Name, string Description);