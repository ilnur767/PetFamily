namespace PetFamily.Accounts.Infrastructure;

public class JwtOptions
{
    public const string Jwt = nameof(Jwt);
    public string Audience { get; init; }
    public string Issuer { get; init; }
    public string Key { get; init; }
    public string ExpiredMinutesTime { get; init; }
}
