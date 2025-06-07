using System.Text;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Common;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Domain.ValueObjects;
using static PetFamily.SharedKernel.Common.ValidationMessageConstants;

namespace PetFamily.Volunteers.Domain.Entities;

public class Pet : SoftDeletableEntity<PetId>
{
    private List<Photo> _photo = [];

    private List<Requisite> _requisite = [];

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

    public string? HealthInformation { get; private set; }

    public string? Address { get; private set; }

    public double? Weight { get; private set; }

    public double? Height { get; private set; }

    public PhoneNumber? PhoneNumber { get; private set; }

    public bool? IsCastrated { get; private set; }

    public DateTime? DateOfBirth { get; private set; }

    public bool? IsVaccinated { get; private set; }

    public PetStatus Status { get; private set; } = default!;

    public IReadOnlyList<Requisite> Requisites
    {
        get => _requisite;
        private set => _requisite = value.ToList();
    }

    public IReadOnlyList<Photo> Photos
    {
        get => _photo;
        private set => _photo = value.ToList();
    }

    public DateTime CreatedAt { get; private set; }

    public Position Position { get; private set; } = default!;

    public void UpdatePhotos(IEnumerable<Photo> photos)
    {
        _photo = photos.ToList();
    }

    public void UpdateRequisites(IEnumerable<Requisite> requisites)
    {
        _requisite = requisites.ToList();
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

    public void UpdateInfo(
        PetSpecies petSpecies,
        string nickName,
        string description,
        PhoneNumber phoneNumber,
        double height,
        double weight,
        bool isCastrated,
        bool isVaccinated,
        DateTime dateOfBirth,
        string healthInformation,
        string address)
    {
        PetSpecies = petSpecies;
        NickName = nickName;
        Description = description;
        PhoneNumber = phoneNumber;
        Weight = weight;
        Height = height;
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
        DateOfBirth = dateOfBirth;
        HealthInformation = healthInformation;
        Address = address;
    }

    public void UpdateStatus(PetStatus status)
    {
        Status = status;
    }

    public UnitResult<Error> UpdateMainPhoto(Photo photo)
    {
        var photoExists = _photo.FirstOrDefault(p => p.FilePath == photo.FilePath);
        if (photoExists is null)
        {
            return Errors.General.NotFound();
        }

        var updatedPhotos = _photo
            .Select(p => Photo.Create(p.FileName, p.FilePath, photo.FilePath == p.FilePath).Value)
            .OrderByDescending(p => p.IsMain)
            .ToList();

        _photo = updatedPhotos;

        return Result.Success<Error>();
    }
}
