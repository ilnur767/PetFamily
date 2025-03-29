using System.Text;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;
using static PetFamily.Domain.Common.ValidationMessageConstants;

namespace PetFamily.Domain.Volunteers;

public class Pet : Entity<PetId>
{
    // ef
    private Pet()
    {
    }

    private Pet(PetId id, string nickName,
        PetSpecies petSpecies,
        PetStatus status,
        string description,
        PhoneNumber phoneNumber) : base(id)
    {
        NickName = nickName;
        PetSpecies = petSpecies;
        Status = status;
        Description = description;
        PhoneNumber = phoneNumber;
        CreatedAt = DateTime.UtcNow;
    }

    public string NickName { get; private set; }

    public PetSpecies PetSpecies { get; private set; }

    public string? Description { get; private set; }

    public string? Color { get; }

    public string? HealthInformation { get; private set; } = default!;

    public string? Address { get; private set; } = default!;

    public double? Weight { get; }

    public double? Height { get; }

    public PhoneNumber? PhoneNumber { get; }

    public bool? IsCastrated { get; }

    public DateTime? DateOfBirth { get; }

    public bool? IsVaccinated { get; }

    public PetStatus Status { get; private set; } = default!;

    public RequisiteList? RequisiteList { get; set; }

    public PetPhotosList? PhotosList { get; set; }

    public DateTime CreatedAt { get; private set; }


    public Position Position { get; private set; } = default!;

    public void AddPhotos(PetPhotosList petPhotosList)
    {
        PhotosList = petPhotosList;
    }

    public void DeletePhotos(PetPhotosList petPhotosList)
    {
        PhotosList = petPhotosList;
    }

    public void SetPosition(Position position)
    {
        Position = position;
    }

    public static Result<Pet> Create(PetId id, string nickName, string description, PhoneNumber phoneNumber,
        PetSpecies petSpecies, PetStatus status)
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

        return Result.Success(new Pet(id, nickName, petSpecies, status, description, phoneNumber));
    }

    public UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();

        if (newPosition.IsFailure)
        {
            return newPosition.Error;
        }

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    public UnitResult<Error> MoveBack()
    {
        var newPosition = Position.Back();

        if (newPosition.IsFailure)
        {
            return newPosition.Error;
        }

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    public void Move(Position newPosition)
    {
        Position = newPosition;
    }
}
