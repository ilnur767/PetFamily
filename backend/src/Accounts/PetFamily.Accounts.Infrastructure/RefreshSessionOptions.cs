namespace PetFamily.Accounts.Infrastructure;

public class RefreshSessionOptions
{
    public const string RefreshSession = nameof(RefreshSession);

    public int ExpiredDaysTime { get; init; }
}
