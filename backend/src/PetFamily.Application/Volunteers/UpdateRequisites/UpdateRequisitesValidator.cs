using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Application.Validation;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

[UsedImplicitly]
public sealed class UpdateRequisitesValidator : AbstractValidator<UpdateRequisitesCommand>
{
    public UpdateRequisitesValidator()
    {
        RuleForEach(u => u.UpdateRequisitesDto)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}