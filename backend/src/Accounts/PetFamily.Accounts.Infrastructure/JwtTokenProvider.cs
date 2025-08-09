using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly AccountsDbContext _accountsDbContext;
    private readonly JwtOptions _jwtOptions;
    private readonly PermissionManager _permissionManager;
    private readonly RefreshSessionOptions _refreshSession;
    private readonly TimeProvider _timeProvider;

    public JwtTokenProvider(
        PermissionManager permissionManager,
        IOptions<JwtOptions> options,
        TimeProvider timeProvider,
        IOptions<RefreshSessionOptions> refreshSession, AccountsDbContext accountsDbContext)
    {
        _permissionManager = permissionManager;
        _timeProvider = timeProvider;
        _accountsDbContext = accountsDbContext;
        _refreshSession = refreshSession.Value;
        _jwtOptions = options.Value;
    }

    public async Task<JwtTokenResult> GenerateAccessToken(User user, CancellationToken cancellationToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signInCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var permissions = await _permissionManager.GetUserPermissions(user.Id, cancellationToken);
        var permissionsClaims = permissions.Select(p => new Claim(CustomClaims.Permission, p));

        var roleClaims = user.Roles.Select(r => new Claim(CustomClaims.Role, r.Name ?? string.Empty));

        var jti = Guid.NewGuid();

        var claims = new[]
            {
                new Claim(CustomClaims.Id, user.Id.ToString()), new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(CustomClaims.Jti, jti.ToString())
            }
            .Concat(permissionsClaims)
            .Concat(roleClaims);

        var expiresDateTime = _timeProvider
            .GetUtcNow()
            .UtcDateTime
            .AddMinutes(int.Parse(_jwtOptions.ExpiredMinutesTime));

        var jwtToken = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            expires: expiresDateTime,
            signingCredentials: signInCredentials,
            claims: claims
        );

        var jwtStringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return new JwtTokenResult(jwtStringToken, jti);
    }

    public async Task<Guid> GenerateRefreshToken(User user, Guid jti, CancellationToken cancellationToken)
    {
        var refreshToken = Guid.NewGuid();

        var refreshSession = new RefreshSession
        {
            UserId = user.Id,
            CreatedAt = _timeProvider.GetUtcNow().UtcDateTime,
            ExpiresIn = _timeProvider.GetUtcNow().UtcDateTime.AddDays(_refreshSession.ExpiredDaysTime),
            RefreshToken = refreshToken,
            Jti = jti
        };

        _accountsDbContext.RefreshSessions.Add(refreshSession);
        await _accountsDbContext.SaveChangesAsync(cancellationToken);

        return refreshToken;
    }

    public async Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaims(string jwtToken, CancellationToken cancellationToken)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var validationParameters = TokenValidationParametersFactory.CreateWithoutLifeTime(_jwtOptions);
        var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validationParameters);

        if (validationResult.IsValid == false)
        {
            return Errors.Tokens.InvalidToken();
        }

        return validationResult.ClaimsIdentity.Claims.ToList();
    }
}
