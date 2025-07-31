using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Abstractions;

public interface ITokenProvider
{
    Task<string> GenerateAccessToken(User user, CancellationToken cancellationToken);
}
