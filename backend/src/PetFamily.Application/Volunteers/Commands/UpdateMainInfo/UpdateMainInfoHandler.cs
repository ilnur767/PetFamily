using CSharpFunctionalExtensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfo;

[UsedImplicitly]
public class UpdateMainInfoHandler : ICommandHandler<Guid, UpdateMainInfoCommand>
{
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IValidator<UpdateMainInfoCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public UpdateMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateMainInfoHandler> logger,
        IValidator<UpdateMainInfoCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validation = await _validator.ValidateAsync(command, cancellationToken);

        if (validation.IsValid == false)
        {
            return validation.ToErrorList();
        }

        var volunteerResult = await _volunteersRepository.GetById(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error.ToErrorList();
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

public record UpdateMainInfoCommand(Guid Id, UpdateMainInfoDto UpdateMainInfoDto) : ICommand;

public record UpdateMainInfoDto(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string PhoneNumber,
    string Description,
    int WorkExperience);
