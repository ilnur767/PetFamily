using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public sealed class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly ILogger<RegisterUserHandler> _logger;
    private readonly IParticipantAccountManager _participantAccountManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public RegisterUserHandler(UserManager<User> userManager, ILogger<RegisterUserHandler> logger, RoleManager<Role> roleManager,
        IParticipantAccountManager participantAccountManager)
    {
        _userManager = userManager;
        _logger = logger;
        _roleManager = roleManager;
        _participantAccountManager = participantAccountManager;
    }

    public async Task<UnitResult<ErrorList>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var existedUser = await _userManager.FindByEmailAsync(command.Email);

        if (existedUser != null)
        {
            return Errors.General.AlreadyExists().ToErrorList();
        }

        var role = await _roleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT);

        if (role == null)
        {
            return Errors.General.NotFound("role participant not found").ToErrorList();
        }

        var user = User.CreateParticipant(command.Email, command.Email, role);

        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded == false)
        {
            var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();

            return new ErrorList(errors);
        }

        await _participantAccountManager.CreateParticipantAccount(new ParticipantAccount { User = user });

        _logger.LogInformation("User created:{userName} a new account with password.", command.UserName);

        return Result.Success<ErrorList>();
    }
}

public record RegisterUserCommand(string Email, string UserName, string Password) : ICommand;
