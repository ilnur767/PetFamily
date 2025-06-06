using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.Common;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.UpdateMainInfo;

[UsedImplicitly]
public sealed class UpdateMainInfoRequestValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoRequestValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        RuleFor(u => u.UpdateMainInfoDto.Description).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        RuleFor(u => u.UpdateMainInfoDto.WorkExperience).NotEmpty().WithError(Errors.General.ValueIsInvalid());
        RuleFor(u => u.UpdateMainInfoDto.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        RuleFor(u => u.UpdateMainInfoDto.Email).MustBeValueObject(Email.Create);
        RuleFor(u => new { u.UpdateMainInfoDto.FirstName, u.UpdateMainInfoDto.LastName, u.UpdateMainInfoDto.MiddleName })
            .MustBeValueObject(c => FullName.Create(c.FirstName, c.LastName, c.MiddleName));
    }
}
