using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Accounts.Application;

public interface IRefreshSessionManager
{
    public Task<Result<RefreshSession, Error>> GetByRefreshToken(Guid refreshToken, CancellationToken cancellationToken);
    void Delete(RefreshSession refreshSession);
}
