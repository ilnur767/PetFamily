using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Core.Validation;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.UpdateSocialMedias;

[UsedImplicitly]
public sealed class UpdateSocialMediasValidator : AbstractValidator<UpdateSocialMediasCommand>
{
    public UpdateSocialMediasValidator()
    {
        RuleForEach(u => u.UpdateSocialMediasDto)
            .MustBeValueObject(r => SocialMedia.Create(r.Name, r.Link));
    }
}
