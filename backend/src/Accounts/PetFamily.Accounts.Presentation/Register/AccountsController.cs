using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Core.Abstractions;
using PetFamily.Framework;

namespace PetFamily.Accounts.Presentation.Register;

public class AccountsController : ControllerBase
{
    private readonly ICommandHandler<RegisterUserCommand> _registerUserCommandHandler;

    public AccountsController(ICommandHandler<RegisterUserCommand> registerUserCommandHandler)
    {
        _registerUserCommandHandler = registerUserCommandHandler;
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _registerUserCommandHandler.Handle(new RegisterUserCommand(request.Email, request.UserName, request.Password), cancellationToken);

        if (!result.IsSuccess)
        {
            return result.Error.ToErrorResponse();
        }

        return NoContent();
    }
}

public record RegisterRequest(string Email, string UserName, string Password);

public record LoginRequest(string Email, string Password);
