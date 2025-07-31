using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Accounts.Application.Commands.Login;

public class LoginHandler : ICommandHandler<string, LoginUserCommand>
{
    private readonly ILogger<LoginHandler> _logger;
    private readonly ITokenProvider _tokenProvider;
    private readonly UserManager<User> _userManager;

    public LoginHandler(UserManager<User> userManager, ILogger<LoginHandler> logger, ITokenProvider tokenProvider)
    {
        _userManager = userManager;
        _logger = logger;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<string, ErrorList>> Handle(LoginUserCommand userCommand, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(userCommand.Email);

        if (user == null)
        {
            return Errors.General.NotFound().ToErrorList();
        }

        var passwordCheck = await _userManager.CheckPasswordAsync(user, userCommand.Password);

        if (!passwordCheck)
        {
            return Errors.User.InvalidCredentials().ToErrorList();
        }

        var token = await _tokenProvider.GenerateAccessToken(user, cancellationToken);

        _logger.LogInformation($"User '{user.Email}' logged in.");

        return token;
    }
}

public record LoginUserCommand(string Email, string Password) : ICommand;
