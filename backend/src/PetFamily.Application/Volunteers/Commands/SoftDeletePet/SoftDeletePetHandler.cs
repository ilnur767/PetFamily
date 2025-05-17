using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Volunteers.Commands.SoftDeletePet;

public sealed class SoftDeletePetHandler : ICommandHandler<SoftDeletePetCommand>
{
    private readonly TimeProvider _timeProvider;
    private readonly IVolunteersRepository _volunteersRepository;

    public SoftDeletePetHandler(IVolunteersRepository volunteersRepository, TimeProvider timeProvider)
    {
        _volunteersRepository = volunteersRepository;
        _timeProvider = timeProvider;
    }

    public async Task<UnitResult<ErrorList>> Handle(SoftDeletePetCommand command, CancellationToken cancellationToken)
    {
        var volunteer = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteer.IsFailure)
        {
            return volunteer.Error.ToErrorList();
        }

        var deleteResult = volunteer.Value.PetSoftDelete(command.PetId, _timeProvider.GetUtcNow().UtcDateTime);

        if (deleteResult.IsFailure)
        {
            return deleteResult.Error.ToErrorList();
        }

        await _volunteersRepository.Save(volunteer.Value, cancellationToken);

        return new UnitResult<ErrorList>();
    }
}

public record SoftDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
