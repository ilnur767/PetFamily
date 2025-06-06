using CSharpFunctionalExtensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Common;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.UpdateSocialMedias;

[UsedImplicitly]
public sealed class UpdateSocialMediasHandler : ICommandHandler<Guid, UpdateSocialMediasCommand>
{
    private readonly ILogger<UpdateSocialMediasHandler> _logger;
    private readonly IValidator<UpdateSocialMediasCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;

    public UpdateSocialMediasHandler(
        ILogger<UpdateSocialMediasHandler> logger,
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateSocialMediasCommand> validator)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateSocialMediasCommand command,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(command, cancellationToken);

        if (validation.IsValid == false)
        {
            return validation.ToErrorList();
        }

        var volunteerResult = await _volunteersRepository.GetById(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error.ToErrorList();
        }

        var volunteer = volunteerResult.Value;
        volunteer.UpdateSocialMedias(
            command.UpdateSocialMediasDto.Select(r => SocialMedia.Create(r.Name, r.Link).Value)
                .ToList());

        await _volunteersRepository.Save(volunteer, cancellationToken);

        _logger.LogInformation("Updated social medias for volunteer with id: {volunteerId}", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}

public record UpdateSocialMediasCommand(Guid Id, IEnumerable<UpdateSocialMediasDto> UpdateSocialMediasDto) : ICommand;

public record UpdateSocialMediasDto(string Name, string Link);
