using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.ChangePetPosition;

public sealed class ChangePetPositionHandler
{
    private readonly IValidator<ChangePetPositionCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public ChangePetPositionHandler(IVolunteersRepository volunteersRepository, IValidator<ChangePetPositionCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(ChangePetPositionCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteer = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteer.IsFailure)
        {
            return volunteer.Error.ToErrorList();
        }

        var pet = volunteer.Value.GetPetById(command.PetId);

        if (pet.IsFailure)
        {
            return pet.Error.ToErrorList();
        }

        var newPosition = Position.Create(command.NewPosition);

        if (newPosition.IsFailure)
        {
            return newPosition.Error.ToErrorList();
        }

        var result = volunteer.Value.MovePet(pet.Value, newPosition.Value);

        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        await _volunteersRepository.Save(volunteer.Value, cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}
