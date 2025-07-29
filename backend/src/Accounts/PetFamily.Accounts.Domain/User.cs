using Microsoft.AspNetCore.Identity;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    private User()
    {
    }

    public IReadOnlyList<SocialMedia>? SocialMedias { get; set; }

    public IReadOnlyList<Role> Roles => _roles;

    private List<Role> _roles { get; set; } = [];

    public static User CreateAdmin(string userName, string email, Role role)
    {
        return new User { UserName = userName, Email = email, _roles = [role] };
    }

    public static User CreateParticipant(string userName, string email, Role role)
    {
        return new User { UserName = userName, Email = email, _roles = [role] };
    }
}
