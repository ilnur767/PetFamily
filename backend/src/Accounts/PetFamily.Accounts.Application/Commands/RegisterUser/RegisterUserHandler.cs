using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetFamily.Accounts.Application.DataModels;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public sealed class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;

    public RegisterUserHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UnitResult<ErrorList>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var existedUser = await _userManager.FindByEmailAsync(command.Email);

        if (existedUser != null)
        {
            return Errors.General.AlreadyExists().ToErrorList();
        }

        var user = new User { Email = command.Email, UserName = command.Email };

        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded == false)
        {
            var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();

            return new ErrorList(errors);
        }

        return Result.Success<ErrorList>();
    }
}

public record RegisterUserCommand(string Email, string UserName, string Password) : ICommand;
