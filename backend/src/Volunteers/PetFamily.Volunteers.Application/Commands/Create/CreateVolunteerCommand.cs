using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Commands.Create;

public record CreateVolunteerCommand(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string PhoneNumber) : ICommand;
