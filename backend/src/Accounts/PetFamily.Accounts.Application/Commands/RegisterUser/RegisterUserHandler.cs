using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel.Common;

namespace PetFamily.Accounts.Application.Commands.RegisterUser;

public sealed class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly ILogger<RegisterUserHandler> _logger;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public RegisterUserHandler(
        UserManager<User> userManager,
        ILogger<RegisterUserHandler> logger,
        RoleManager<Role> roleManager,
        [FromKeyedServices(UnitOfWorkTypes.Accounts)]
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
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

        var user = User.CreateParticipant(command.Email, command.Email);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var result = await _userManager.CreateAsync(user, command.Password);

            if (result.Succeeded == false)
            {
                var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();

                await _unitOfWork.RollbackAsync(cancellationToken);

                return new ErrorList(errors);
            }

            var roleResult =await _userManager.AddToRoleAsync(user, ParticipantAccount.PARTICIPANT);

            if (roleResult.Succeeded == false)
            {
                var errors = roleResult.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();

                await _unitOfWork.RollbackAsync(cancellationToken);

                return new ErrorList(errors);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            await _unitOfWork.RollbackAsync(cancellationToken);
        }


        _logger.LogInformation("User created:{userName} a new account with password.", command.UserName);

        return Result.Success<ErrorList>();
    }
}

public record RegisterUserCommand(string Email, string UserName, string Password) : ICommand;
