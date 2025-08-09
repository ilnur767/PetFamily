using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.RefreshTokens;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Presentation.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;

namespace PetFamily.Accounts.Presentation.Refresh;

public class AccountsController : ControllerBase
{
    private readonly ICommandHandler<LoginResponse, RefreshTokensCommand> _refreshTokenCommandHandler;

    public AccountsController(
        ICommandHandler<LoginResponse, RefreshTokensCommand> refreshTokenCommandHandler)
    {
        _refreshTokenCommandHandler = refreshTokenCommandHandler;
    }

    /// <summary>
    ///     Получить обновленный токен доступа.
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokensRequest refreshTokensRequest,
        CancellationToken cancellationToken)
    {
        var result = await _refreshTokenCommandHandler.Handle(new RefreshTokensCommand(refreshTokensRequest.AccessToken, refreshTokensRequest.RefreshToken),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return result.Error.ToErrorResponse();
        }

        Response.Cookies.SetRefreshToken(result.Value.RefreshToken);

        return Ok(Envelop.Ok(result.Value));
    }
}
