// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.Commands.UpdatePetStatus;

public sealed class UpdatePetStatusHandler : ICommandHandler<UpdatePetStatusCommand>
{
    private readonly IValidator<UpdatePetStatusCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;


    public UpdatePetStatusHandler(IVolunteersRepository volunteersRepository, IValidator<UpdatePetStatusCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
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

        return new UnitResult<ErrorList>();
    }
}

public record UpdatePetStatusCommand(Guid VolunteerId, Guid PetId, string Status) : ICommand;
