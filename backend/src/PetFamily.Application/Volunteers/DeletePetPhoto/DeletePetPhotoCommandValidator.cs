// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Application.Validation;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Volunteers.DeletePetPhoto;

[UsedImplicitly]
public sealed class DeletePetPhotoCommandValidator : AbstractValidator<DeletePetPhotoCommand>
{
    public DeletePetPhotoCommandValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(d => d.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(d => d.FilesPath).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
