using CSharpFunctionalExtensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Common;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.UpdatePetStatus;

[UsedImplicitly]
public sealed class UpdatePetStatusHandler : ICommandHandler<UpdatePetStatusCommand>
{
    private readonly ILogger<UpdatePetStatusHandler> _logger;
    private readonly IValidator<UpdatePetStatusCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public UpdatePetStatusHandler(IVolunteersRepository volunteersRepository, IValidator<UpdatePetStatusCommand> validator,
        ILogger<UpdatePetStatusHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(UpdatePetStatusCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorList();
        }

        var volunteer = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return volunteer.Error.ToErrorList();
        }

        var petStatus = PetStatus.Create(command.Status);

        volunteer.Value.UpdatePetStatus(command.PetId, petStatus.Value);

        await _volunteersRepository.Save(volunteer.Value, cancellationToken);

        _logger.LogInformation("Updated status for pet with id: {id}", command.PetId);

        return Result.Success<ErrorList>();
    }
}

public record UpdatePetStatusCommand(Guid VolunteerId, Guid PetId, string Status) : ICommand;
