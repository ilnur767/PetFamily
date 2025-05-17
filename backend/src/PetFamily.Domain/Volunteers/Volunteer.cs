using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;

namespace PetFamily.Domain.Volunteers;

public class Volunteer : SoftDeletableEntity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    // ef
    private Volunteer()
    {
    }

    public Volunteer(VolunteerId id, FullName fullName, Email email, PhoneNumber phoneNumber) : base(id)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public FullName FullName { get; private set; }

    public Email Email { get; private set; }

    public string? Description { get; private set; }

    public int? WorkExperience { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public SocialMediaList? SocialMediasList { get; private set; }

    public RequisiteList? RequisiteList { get; private set; }

    public IReadOnlyList<Pet> Pets => _pets;

    public int PetsFoundHomeCount => _pets.Count(p => p.Status.Status == PetStatusValue.FoundHome);
    public int PetsLookingForHomeCount => _pets.Count(p => p.Status.Status == PetStatusValue.LookingForHome);
    public int PetsNeedsHelpCount => _pets.Count(p => p.Status.Status == PetStatusValue.NeedsHelp);

    public void UpdateRequisites(IEnumerable<Requisite> requisites)
    {
        RequisiteList = RequisiteList.Create(requisites.ToList());
    }

    public void UpdateSocialMedias(IEnumerable<SocialMedia> socialMedias)
    {
        SocialMediasList = SocialMediaList.Create(socialMedias.ToList());
    }

    public void UpdateMainInfo(FullName fullName, string? description, int? workExperience, PhoneNumber phoneNumber,
        Email email)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        Description = description;
        WorkExperience = workExperience;
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        var serialNumber = Position.Create(_pets.Count + 1);

        if (serialNumber.IsFailure)
        {
            return serialNumber.Error;
        }

        pet.SetPosition(serialNumber.Value);

        _pets.Add(pet);

        return new UnitResult<Error>();
    }

    public UnitResult<Error> UpdatePet(
        Guid petId,
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
        var existPet = Pets.FirstOrDefault(p => p.Id.Value == petId);

        if (existPet == null)
        {
            return Errors.General.NotFound(petId);
        }

        existPet.UpdateInfo(petSpecies, nickName, description, phoneNumber, height, weight, isCastrated, isVaccinated, dateOfBirth, healthInformation, address);

        return new UnitResult<Error>();
    }

    public Result<Pet, Error> GetPetById(Guid id)
    {
        var pet = _pets.FirstOrDefault(p => p.Id.Value == id);

        if (pet == null)
        {
            return Errors.General.NotFound(id);
        }

        return pet;
    }

    public void UpdateRequisites(SocialMediaList socialMediasList)
    {
        SocialMediasList = socialMediasList;
    }

    public UnitResult<Error> MovePet(Pet pet, Position newPosition)
    {
        var currentPosition = pet.Position;

        if (currentPosition == newPosition || _pets.Count == 1)
        {
            return Result.Success<Error>();
        }

        var adjustPosition = AdjustNewPositionIfOutOfRange(newPosition);

        if (adjustPosition.IsFailure)
        {
            return adjustPosition.Error;
        }

        newPosition = adjustPosition.Value;

        var moveResult = MovePetsBetweenPositions(newPosition, currentPosition);
        if (moveResult.IsFailure)
        {
            return moveResult.Error;
        }

        pet.Move(newPosition);

        return Result.Success<Error>();
    }

    private UnitResult<Error> MovePetsBetweenPositions(Position newPosition, Position currentPosition)
    {
        if (newPosition < currentPosition)
        {
            var petsToMove = _pets.Where(p => p.Position >= newPosition
                                              && p.Position < currentPosition);
            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveForward();

                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
        }
        else if (newPosition > currentPosition)
        {
            var petsToMove = _pets.Where(p => p.Position > currentPosition
                                              && p.Position <= newPosition);
            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveBack();

                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
        }

        return Result.Success<Error>();
    }

    private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition <= _pets.Count)
        {
            return newPosition;
        }

        var lastPosition = Position.Create(_pets.Count - 1);
        if (lastPosition.IsFailure)
        {
            return lastPosition.Error;
        }

        return lastPosition;
    }
}
