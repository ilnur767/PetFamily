using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Create;

[UsedImplicitly]
public sealed class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);

        RuleFor(c => new { c.FirstName, c.LastName, c.MiddleName })
            .MustBeValueObject(c => FullName.Create(c.FirstName, c.LastName, c.MiddleName));
    }
}
