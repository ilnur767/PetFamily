using System.Text;
using CSharpFunctionalExtensions;
using static PetFamily.Domain.Common.ValidationMessageConstants;

namespace PetFamily.Domain.Volunteers;

public class Pet : Entity<PetId>
{
    private readonly List<Requisite> _requisite = [];

    private Pet(PetId id, string nickName, PetSpecies petSpecies, PetStatus status) : base(id)
    {
        NickName = nickName;
        PetSpecies = petSpecies;
        Status = status;
        CreatedAt = DateTime.UtcNow;
    }

    public string NickName { get; private set; }

    public PetSpecies PetSpecies { get; private set; }

    public string? Description { get; private set; } = default!;

    public string? Color { get; private set;}

    public string? HealthInformation { get; private set; } = default!;

    public string? Address { get; private set; } = default!;

    public double? Weight { get; private set;}

    public double? Height { get; private set;}

    public PhoneNumber? PhoneNumber { get; private set; }

    public bool? IsCastrated { get; private set;}

    public DateTime? DateOfBirth { get; private set;}

    public bool? IsVaccinated { get; private set;}

    public PetStatus Status { get; private set; } = default!;

    public IReadOnlyList<Requisite> Requisites => _requisite;

    public DateTime CreatedAt { get; private set; }

    public static Result<Pet> Create(PetId id, string nickName, PetSpecies petSpecies, PetStatus status)
    {
        var errorMessage = new StringBuilder();

        if (string.IsNullOrEmpty(nickName))
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, nameof(NickName)));
        }

        if (petSpecies is null)
        {
            errorMessage.AppendLine(string.Format(EmptyPropertyTemplate, nameof(PetSpecies)));
        }

        if (errorMessage.Length > 0)
        {
            return Result.Failure<Pet>(errorMessage.ToString());
        }

        return Result.Success<Pet>(new Pet(id, nickName, petSpecies, status));
    }
}