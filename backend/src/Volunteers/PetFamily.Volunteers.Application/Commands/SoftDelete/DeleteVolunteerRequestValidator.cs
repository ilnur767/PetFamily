using FluentValidation;
using JetBrains.Annotations;

namespace PetFamily.Volunteers.Application.Commands.SoftDelete;

[UsedImplicitly]
public sealed class DeleteVolunteerRequestValidator : AbstractValidator<SoftDeleteVolunteerCommand>
{
    public DeleteVolunteerRequestValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty();
    }
}
