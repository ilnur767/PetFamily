// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;
using static PetFamily.Domain.Common.DataLimitsConstants;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        RuleFor(a => a.PetStatus).MustBeValueObject(PetStatus.Create);

        RuleFor(a => a.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleFor(a => a.NickName)
            .NotEmpty()
            .MaximumLength(MaxLowTextLength)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(a => a.Description)
            .NotEmpty()
            .MaximumLength(MaxHighTextLength)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(a => a.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsInvalid());

        RuleFor(a => a.BreedId).NotEmpty().WithError(Errors.General.ValueIsInvalid());
    }
}
