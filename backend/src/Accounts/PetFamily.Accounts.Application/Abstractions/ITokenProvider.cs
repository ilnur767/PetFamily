using PetFamily.Accounts.Application.DataModels;

namespace PetFamily.Accounts.Application.Abstractions;

public interface ITokenProvider
{
    string GenerateAccessToken(User user, CancellationToken cancellationToken);
}
