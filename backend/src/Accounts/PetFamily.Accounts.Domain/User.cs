using Microsoft.AspNetCore.Identity;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    private User()
    {
    }

    public IReadOnlyList<SocialMedia>? SocialMedias { get; set; }

    public AdminAccount AdminAccount { get; set; }

    public VolunteerAccount VolunteerAccount { get; set; }

    public ParticipantAccount ParticipantAccount { get; set; }

    public FullName FullName { get; set; } = default!;

    public IReadOnlyList<Role> Roles => _roles;

    private List<Role> _roles { get; set; } = [];

    public static User CreateAdmin(string userName, string email)
    {
        var user = new User
        {
            FullName = FullName.Create(userName,userName,userName).Value,
            UserName = userName,
            Email = email
        };

        var admin = new AdminAccount(user);

        user.AdminAccount = admin;

        return user;
    }

    public static User CreateParticipant(string userName, string email)
    {
        var user = new User
        {
            FullName = FullName.Create(userName,userName,userName).Value,
            UserName = userName,
            Email = email
        };

        var participant = new ParticipantAccount();

        user.ParticipantAccount = participant;

        return user;
    }

    public static User CreateVolunteer(string userName, string email)
    {
        var user = new User { UserName = userName, Email = email};

        var volunteer = new VolunteerAccount();

        user.VolunteerAccount = volunteer;

        return user;
    }
}
