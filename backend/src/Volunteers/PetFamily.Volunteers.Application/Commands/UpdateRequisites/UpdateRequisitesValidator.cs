using FluentValidation;
using JetBrains.Annotations;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.UpdateRequisites;

[UsedImplicitly]
public sealed class UpdateRequisitesValidator : AbstractValidator<UpdateRequisitesCommand>
{
    public UpdateRequisitesValidator()
    {
        RuleForEach(u => u.UpdateRequisitesDto)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}
