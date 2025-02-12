using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;

    public UpdateMainInfoHandler(IVolunteersRepository volunteersRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(UpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error;
        }

        var volunteer = volunteerResult.Value;

        var fullName = FullName.Create(command.UpdateMainInfoDto.FirstName, command.UpdateMainInfoDto.LastName,
            command.UpdateMainInfoDto.MiddleName).Value;
        var email = Email.Create(command.UpdateMainInfoDto.Email).Value;

        var phoneNumber = PhoneNumber.Create(command.UpdateMainInfoDto.PhoneNumber).Value;

        volunteer.UpdateMainInfo(
            fullName,
            command.UpdateMainInfoDto.Description,
            command.UpdateMainInfoDto.WorkExperience,
            phoneNumber, email);

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        _logger.LogInformation("Updated volunteer with id: {volunteerId}", volunteer.Id.Value);

        return volunteerResult.Value.Id.Value;
    }
}

public record UpdateMainInfoCommand(Guid Id, UpdateMainInfoDto UpdateMainInfoDto);

public record UpdateMainInfoDto(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string PhoneNumber,
    string Description,
    int WorkExperience);