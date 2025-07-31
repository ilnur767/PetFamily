using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class VolunteerAccount
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string? Certificates { get; set; }
    public int? WorkExperience { get; set; }
    public IReadOnlyList<Requisite>? Requisites { get; set; }
}
