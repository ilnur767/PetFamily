// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.ChangePetPosition;

public sealed class ChangePetPositionHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public ChangePetPositionHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task<UnitResult<Error>> Handle(ChangePetPositionCommand command, CancellationToken cancellationToken)
    {
        var volunteer = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteer.IsFailure)
        {
            return volunteer.Error;
        }

        var pet = volunteer.Value.GetPetById(command.PetId);

        if (pet.IsFailure)
        {
            return pet.Error;
        }

        var newPosition = Position.Create(command.NewPosition);

        if (newPosition.IsFailure)
        {
            return newPosition.Error;
        }

        var result = volunteer.Value.MovePet(pet.Value, newPosition.Value);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await _volunteersRepository.Save(volunteer.Value, cancellationToken);

        return UnitResult.Success<Error>();
    }
}
