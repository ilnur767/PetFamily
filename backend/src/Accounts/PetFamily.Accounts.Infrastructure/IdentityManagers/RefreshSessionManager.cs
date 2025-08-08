using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class RefreshSessionManager : IRefreshSessionManager
{
    private readonly AccountsDbContext _accountsDbContext;

    public RefreshSessionManager(AccountsDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }

    public async Task<Result<RefreshSession, Error>> GetByRefreshToken(Guid refreshToken, CancellationToken cancellationToken)
    {
        var refreshSession = await _accountsDbContext.RefreshSessions
            .Include(rs => rs.User)
            .FirstOrDefaultAsync(rs => rs.RefreshToken == refreshToken, cancellationToken);

        if (refreshSession == null)
        {
            return Errors.General.NotFound(refreshToken);
        }

        return refreshSession;
    }

    public void Delete(RefreshSession refreshSession)
    {
        _accountsDbContext.RefreshSessions.Remove(refreshSession);
    }
}
