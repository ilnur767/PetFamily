using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AccountsDbContext _accountsDbContext;

    public UnitOfWork(AccountsDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        await _accountsDbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _accountsDbContext.SaveChangesAsync(cancellationToken);
        await _accountsDbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _accountsDbContext.Database.RollbackTransactionAsync(cancellationToken);
    }
}
