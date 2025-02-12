using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Common;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.UpdateSocialMedias;

public sealed class UpdateSocialMediasHandler
{
    private readonly ILogger<UpdateSocialMediasHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;

    public UpdateSocialMediasHandler(ILogger<UpdateSocialMediasHandler> logger, IVolunteersRepository volunteersRepository)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, Error>> Handle(UpdateSocialMediasCommand command,
        CancellationToken cancellationToken)
    {
        var volunteerResult = await _volunteersRepository.GetById(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error;
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

public record UpdateSocialMediasCommand(Guid Id, IEnumerable<UpdateSocialMediasDto> UpdateSocialMediasDto);

public record UpdateSocialMediasDto(string Name, string Link);