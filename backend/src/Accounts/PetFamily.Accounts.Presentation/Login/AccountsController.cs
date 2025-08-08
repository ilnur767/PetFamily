using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Presentation.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;

namespace PetFamily.Accounts.Presentation.Login;

public class AccountsController : ControllerBase
{
    private readonly ICommandHandler<LoginResponse, LoginUserCommand> _loginUserCommandHandler;

    public AccountsController(ICommandHandler<LoginResponse, LoginUserCommand> loginUserCommandHandler)
    {
        _loginUserCommandHandler = loginUserCommandHandler;
    }

    /// <summary>
    ///     Выполнить вход и получить токен доступа.
    /// </summary>
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

        Response.Cookies.SetRefreshToken(result.Value.RefreshToken);

        return Ok(Envelop.Ok(result.Value));
    }
}

public record LoginUserRequest(string Email, string Password);
