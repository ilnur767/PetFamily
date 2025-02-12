using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Application.Validation;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateSocialMedias;

[UsedImplicitly]
public sealed class UpdateSocialMediasValidator : AbstractValidator<UpdateSocialMediasCommand>
{
    public UpdateSocialMediasValidator()
    {
        RuleForEach(u => u.UpdateSocialMediasDto)
            .MustBeValueObject(r => SocialMedia.Create(r.Name, r.Link));
    }
}