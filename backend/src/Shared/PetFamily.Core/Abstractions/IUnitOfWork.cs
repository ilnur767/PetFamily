using System.Data;

namespace PetFamily.Core.Abstractions;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);
}
