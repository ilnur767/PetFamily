﻿using CSharpFunctionalExtensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Common;
using PetFamily.Volunteers.Application.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.UpdateRequisites;

[UsedImplicitly]
public sealed class UpdateRequisitesHandler : ICommandHandler<Guid, UpdateRequisitesCommand>
{
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IValidator<UpdateRequisitesCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public UpdateRequisitesHandler(
        ILogger<UpdateMainInfoHandler> logger,
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateRequisitesCommand> validator)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateRequisitesCommand command, CancellationToken cancellationToken)
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
        volunteer.UpdateRequisites(
            command.UpdateRequisitesDto.Select(r => Requisite.Create(r.Name, r.Description).Value)
                .ToList());

        await _volunteersRepository.Save(volunteer, cancellationToken);

        _logger.LogInformation("Updated requisites for volunteer with id: {volunteerId}", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}

public record UpdateRequisitesCommand(Guid Id, IEnumerable<UpdateRequisiteDto> UpdateRequisitesDto) : ICommand;

public record UpdateRequisiteDto(string Name, string Description);
