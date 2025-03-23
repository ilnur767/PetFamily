// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Application.Validation;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Volunteers.AddPetPhoto;

[UsedImplicitly]
public sealed class AddPetPhotoCommandValidator : AbstractValidator<AddPetPhotoCommand>
{
    private readonly long _maxPhotoSize = 10 * 1024 * 1024; // 10MB

    public AddPetPhotoCommandValidator()
    {
        RuleFor(a => a.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        ;
        RuleFor(a => a.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        ;
        RuleFor(a => a.Photos).NotEmpty().WithError(Errors.General.ValueIsRequired());
        ;

        RuleForEach(a => a.Photos)
            .Must(f => f.Content.Length <= _maxPhotoSize).WithError(Errors.General.ValueIsInvalid());
        ;
    }
}
