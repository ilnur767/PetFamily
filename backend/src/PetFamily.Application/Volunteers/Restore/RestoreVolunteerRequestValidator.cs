using FluentValidation;
using JetBrains.Annotations;

namespace PetFamily.Application.Volunteers.Restore;

[UsedImplicitly]
public sealed class RestoreVolunteerRequestValidator : AbstractValidator<RestoreVolunteerRequest>
{
    public RestoreVolunteerRequestValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty();
    }
}