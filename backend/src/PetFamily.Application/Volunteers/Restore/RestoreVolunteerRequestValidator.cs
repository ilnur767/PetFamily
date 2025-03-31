﻿using FluentValidation;
using JetBrains.Annotations;

namespace PetFamily.Application.Volunteers.Restore;

[UsedImplicitly]
public sealed class RestoreVolunteerRequestValidator : AbstractValidator<RestoreVolunteerCommand>
{
    public RestoreVolunteerRequestValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty();
    }
}
