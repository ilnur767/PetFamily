using FluentValidation;
using JetBrains.Annotations;

namespace PetFamily.Application.Volunteers.SoftDelete;

[UsedImplicitly]
public sealed class DeleteVolunteerRequestValidator : AbstractValidator<SoftDeleteVolunteerRequest>
{
    public DeleteVolunteerRequestValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty();
    }
}