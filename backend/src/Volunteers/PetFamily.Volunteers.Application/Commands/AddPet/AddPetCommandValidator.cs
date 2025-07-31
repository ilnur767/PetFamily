using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.Common;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects;
using static PetFamily.SharedKernel.Common.DataLimitsConstants;

namespace PetFamily.Volunteers.Application.Commands.AddPet;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        RuleFor(a => a.PetStatus).MustBeValueObject(PetStatus.Create);

        RuleFor(a => a.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleFor(a => a.NickName)
            .NotEmpty()
            .MaximumLength(MaxLowTextLength)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(a => a.Description)
            .NotEmpty()
            .MaximumLength(MaxHighTextLength)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(a => a.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsInvalid());

        RuleFor(a => a.BreedId).NotEmpty().WithError(Errors.General.ValueIsInvalid());
    }
}
