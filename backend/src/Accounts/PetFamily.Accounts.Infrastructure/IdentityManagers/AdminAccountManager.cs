using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class AdminAccountManager
{
    private readonly AccountsDbContext _accountsDbContext;

    public AdminAccountManager(AccountsDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }

    public async Task CreateAdminAccount(AdminAccount adminAccount)
    {
        _accountsDbContext.AdminAccounts.Add(adminAccount);
        await _accountsDbContext.SaveChangesAsync();
    }
}
