using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Accounts.Application.Commands.RefreshTokens;

public class RefreshTokensHandler : ICommandHandler<LoginResponse, RefreshTokensCommand>
{
    private readonly ILogger<RefreshTokensHandler> _logger;
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly TimeProvider _timeProvider;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokensHandler(IRefreshSessionManager refreshSessionManager,
        ITokenProvider tokenProvider,
        TimeProvider timeProvider,
        [FromKeyedServices(UnitOfWorkTypes.Accounts)]
        IUnitOfWork unitOfWork, ILogger<RefreshTokensHandler> logger)
    {
        _refreshSessionManager = refreshSessionManager;
        _tokenProvider = tokenProvider;
        _timeProvider = timeProvider;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<LoginResponse, ErrorList>> Handle(RefreshTokensCommand command, CancellationToken cancellationToken)
    {
        var oldRefreshSession = await _refreshSessionManager.GetByRefreshToken(command.RefreshToken, cancellationToken);

        if (oldRefreshSession.IsFailure)
        {
            return oldRefreshSession.Error.ToErrorList();
        }

        if (oldRefreshSession.Value.ExpiresIn < _timeProvider.GetUtcNow().UtcDateTime)
        {
            return Errors.Tokens.ExpiredToken().ToErrorList();
        }

        var userClaims = await _tokenProvider.GetUserClaims(command.AccessToken, cancellationToken);

        if (userClaims.IsFailure)
        {
            return userClaims.Error.ToErrorList();
        }

        var userIdString = userClaims.Value.FirstOrDefault(u => u.Type == CustomClaims.Id)?.Value;

        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Errors.General.Failure().ToErrorList();
        }

        if (oldRefreshSession.Value.UserId != userId)
        {
            return Errors.Tokens.InvalidToken().ToErrorList();
        }

        var userJtiString = userClaims.Value.FirstOrDefault(u => u.Type == CustomClaims.Jti)?.Value;
        if (!Guid.TryParse(userJtiString, out var userJtiGuid))
        {
            return Errors.Tokens.InvalidToken().ToErrorList();
        }

        if (oldRefreshSession.Value.Jti != userJtiGuid)
        {
            return Errors.Tokens.InvalidToken().ToErrorList();
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        _refreshSessionManager.Delete(oldRefreshSession.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var accessToken = await _tokenProvider.GenerateAccessToken(oldRefreshSession.Value.User, cancellationToken);
        var refreshToken = await _tokenProvider.GenerateRefreshToken(oldRefreshSession.Value.User, accessToken.Jti, cancellationToken);

        _logger.LogInformation("Tokens refreshed successfully for user {UserId}", userId);

        return new LoginResponse(accessToken.AccessToken, refreshToken);
    }
}

public record RefreshTokensCommand(string AccessToken, Guid RefreshToken) : ICommand;
