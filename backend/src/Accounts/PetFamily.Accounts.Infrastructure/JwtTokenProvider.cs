using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Core.Models;

namespace PetFamily.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly PermissionManager _permissionManager;
    private readonly TimeProvider _timeProvider;

    public JwtTokenProvider(
        PermissionManager permissionManager,
        IOptions<JwtOptions> options,
        TimeProvider timeProvider)
    {
        _permissionManager = permissionManager;
        _timeProvider = timeProvider;
        _jwtOptions = options.Value;
    }

    public async Task<string> GenerateAccessToken(User user, CancellationToken cancellationToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signInCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var permissions = await _permissionManager.GetUserPermissions(user.Id);
        var permissionsClaims = permissions.Select(p => new Claim(CustomClaims.Permission, p));

        var roleClaims = user.Roles.Select(r => new Claim(CustomClaims.Role, r.Name ?? string.Empty));

        var claims = new[] { new Claim(CustomClaims.Id, user.Id.ToString()), new Claim(JwtRegisteredClaimNames.Email, user.Email!) }
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

        var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return stringToken;
    }
}
