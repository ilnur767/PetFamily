using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.UpdatePetStatus;

public sealed class UpdatePetStatusCommandValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusCommandValidator()
    {
        RuleFor(u => u.Status).MustBeValueObject(PetStatus.Create);
    }
}
