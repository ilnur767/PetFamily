using FluentValidation;

namespace PetFamily.Application.Volunteers.SoftDelete;

public sealed class DeleteVolunteerRequestValidator : AbstractValidator<SoftDeleteVolunteerRequest>
{
    public DeleteVolunteerRequestValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty();
    }
}