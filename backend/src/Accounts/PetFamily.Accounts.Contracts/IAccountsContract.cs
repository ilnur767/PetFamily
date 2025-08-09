namespace PetFamily.Accounts.Contracts;

public interface IAccountsContract
{
    Task<HashSet<string>> GetUserPermissionCodes(Guid userId, CancellationToken cancellationToken);
}
