using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;

namespace PetFamily.Accounts.Presentation.Login;

public class AccountsController : ControllerBase
{
    private readonly ICommandHandler<string, LoginUserCommand> _loginUserCommandHandler;

    public AccountsController(ICommandHandler<string, LoginUserCommand> loginUserCommandHandler)
    {
        _loginUserCommandHandler = loginUserCommandHandler;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginUserRequest userRequest,
        CancellationToken cancellationToken)
    {
        var result = await _loginUserCommandHandler.Handle(new LoginUserCommand(userRequest.Email, userRequest.Password), cancellationToken);

        if (!result.IsSuccess)
        {
            return result.Error.ToErrorResponse();
        }

        return Ok(result.Value);
    }
}

public record RegisterRequest(string Email, string UserName, string Password);

public record LoginUserRequest(string Email, string Password);
