using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Providers;

public interface IParticipantAccountManager
{
    Task CreateParticipantAccount(ParticipantAccount participantAccount);
}
