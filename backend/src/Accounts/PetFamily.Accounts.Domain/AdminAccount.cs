using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class AdminAccount
{
    public const string ADMIN = nameof(ADMIN);

    // ef core
    private AdminAccount()
    {
    }

    public AdminAccount(FullName fullName, User user)
    {
        Id = Guid.NewGuid();
        User = user;
        FullName = fullName;
    }

    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public FullName FullName { get; set; } = default!;
}
