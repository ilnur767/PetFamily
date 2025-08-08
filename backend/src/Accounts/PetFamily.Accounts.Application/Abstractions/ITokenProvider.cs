using System.Security.Claims;
using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Accounts.Application.Abstractions;

public interface ITokenProvider
{
    Task<JwtTokenResult> GenerateAccessToken(User user, CancellationToken cancellationToken);
    Task<Guid> GenerateRefreshToken(User user, Guid jti, CancellationToken cancellationToken);
    Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaims(string jwtToken, CancellationToken cancellationToken);
}
