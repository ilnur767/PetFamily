using System.Text;
using CSharpFunctionalExtensions;
using static PetFamily.Domain.Common.ValidationMessageConstants;

namespace PetFamily.Domain.Volunteers;

public class Volunteer : Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];
    private readonly List<Requisite> _requisites = [];
    private readonly List<SocialMedia> _socialMedias = [];

    public Volunteer(VolunteerId id, FullName fullName, Email email, PhoneNumber phoneNumber) : base(id)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public FullName FullName { get; private set; }

    public Email Email { get; private set; }

    public string? Description { get; private set; }

    public int? WorkExperienceExperience { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public IReadOnlyList<Pet> Pets => _pets;
    public IReadOnlyList<Requisite> Requisites => _requisites;
    public IReadOnlyList<SocialMedia> SocialMedias => _socialMedias;

    public int PetsFoundHomeCount => _pets.Count(p => p.Status == PetStatus.FoundHome);
    public int PetsLookingForHomeCount => _pets.Count(p => p.Status == PetStatus.LookingForHome);
    public int PetsNeedsHelpCount => _pets.Count(p => p.Status == PetStatus.NeedsHelp);

    public static Result<Volunteer> Create(VolunteerId id, FullName fullName, Email email, PhoneNumber phoneNumber)
    {
        var errorMessage = new StringBuilder();
        
        if (fullName is null)
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, nameof(FullName)));
        }

        if (email is null)
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, nameof(Email)));
        }

        if (phoneNumber is null)
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, nameof(PhoneNumber)));
        }

        if (errorMessage.Length > 0)
        {
            return Result.Failure<Volunteer>(errorMessage.ToString());
        }

        return Result.Success(new Volunteer(id, fullName!, email!, phoneNumber!));
    }
}