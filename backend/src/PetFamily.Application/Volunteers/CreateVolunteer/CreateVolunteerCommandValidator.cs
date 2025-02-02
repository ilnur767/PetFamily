using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Application.Validation;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

[UsedImplicitly]
public sealed class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);

        RuleFor(c => new { c.FirstName, c.LastName, c.MiddleName })
            .MustBeValueObject(c => FullName.Create(c.FirstName, c.LastName, c.MiddleName));

        RuleForEach(c => c.SocialMedias)
            .MustBeValueObject(s => SocialMedia.Create(s.Name, s.Link));

        RuleForEach(c => c.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}