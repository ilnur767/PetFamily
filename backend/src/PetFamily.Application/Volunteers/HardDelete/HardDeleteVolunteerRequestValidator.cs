using FluentValidation;
using JetBrains.Annotations;

namespace PetFamily.Application.Volunteers.HardDelete;

[UsedImplicitly]
public sealed class HardDeleteVolunteerRequestValidator : AbstractValidator<HardDeleteVolunteerCommand>
{
    public HardDeleteVolunteerRequestValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty();
    }
}
