using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class ParticipantAccountManager : IParticipantAccountManager

{
    private readonly AccountsDbContext _accountsDbContext;

    public ParticipantAccountManager(AccountsDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }

    public async Task CreateParticipantAccount(ParticipantAccount participantAccount)
    {
        _accountsDbContext.ParticipantAccounts.Add(participantAccount);
        await _accountsDbContext.SaveChangesAsync();
    }
}
