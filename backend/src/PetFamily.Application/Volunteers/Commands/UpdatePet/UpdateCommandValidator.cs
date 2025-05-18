using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Application.Validation;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;
using static PetFamily.Domain.Common.DataLimitsConstants;

namespace PetFamily.Application.Volunteers.Commands.UpdatePet;

[UsedImplicitly]
public sealed class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetCommandValidator(TimeProvider timeProvider)
    {
        RuleFor(u => u.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(u => u.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(u => u.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleFor(u => u.NickName)
            .NotEmpty()
            .MaximumLength(MaxLowTextLength)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(u => u.Description)
            .NotEmpty()
            .MaximumLength(MaxHighTextLength)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(u => u.Address)
            .NotEmpty()
            .MaximumLength(MaxHighTextLength)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(u => u.HealthInformation)
            .NotEmpty()
            .MaximumLength(MaxHighTextLength)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(u => u.Height)
            .NotEmpty()
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(u => u.Weight)
            .NotEmpty()
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(u => u.DateOfBirth)
            .NotEmpty()
            .LessThan(timeProvider.GetUtcNow().DateTime)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(u => u.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsInvalid());

        RuleFor(u => u.BreedId).NotEmpty().WithError(Errors.General.ValueIsInvalid());
    }
}
