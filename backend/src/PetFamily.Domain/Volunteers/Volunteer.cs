﻿using System.Text;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;
using static PetFamily.Domain.Common.ValidationMessageConstants;
using static PetFamily.Domain.Common.Errors;

namespace PetFamily.Domain.Volunteers;

public class Volunteer : Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    // ef
    private Volunteer() { }
    
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

    public int PetsFoundHomeCount => _pets.Count(p => p.Status == PetStatus.FoundHome);
    public int PetsLookingForHomeCount => _pets.Count(p => p.Status == PetStatus.LookingForHome);
    public int PetsNeedsHelpCount => _pets.Count(p => p.Status == PetStatus.NeedsHelp);

    public void AddRequisites(IEnumerable<Requisite> requisites)
    {
        RequisiteList = RequisiteList.Create(requisites.ToList());
    }
    
    public void AddSocialMedias(IEnumerable<SocialMedia> socialMedias)
    {
        SocialMediasList = SocialMediaList.Create(socialMedias.ToList());
    }
}