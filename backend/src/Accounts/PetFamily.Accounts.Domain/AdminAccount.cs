using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class AdminAccount
{
    public const string ADMIN = nameof(ADMIN);

    // ef core
    private AdminAccount()
    {
    }

    public AdminAccount(User user)
    {
        Id = Guid.NewGuid();
        User = user;
    }

    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
}
