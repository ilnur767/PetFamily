﻿// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.Common;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.ChangePetPosition;

[UsedImplicitly]
public sealed class ChangePetPositionCommandValidator : AbstractValidator<ChangePetPositionCommand>
{
    public ChangePetPositionCommandValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.NewPosition).MustBeValueObject(Position.Create);
    }
}
