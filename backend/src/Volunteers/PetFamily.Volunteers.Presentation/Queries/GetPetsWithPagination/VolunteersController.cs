using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using PetFamily.Framework.Authorization;
using PetFamily.Volunteers.Application.Queries.GetPetsWithPagination;

namespace PetFamily.Volunteers.Presentation.Queries.GetPetsWithPagination;

[Authorize]
[ApiController]
[Route("[controller]")]
[Permission(PermissionTypes.Volunteers.Read)]
public class VolunteersController : ControllerBase
{
    /// <summary>
    ///     Получение списка питомцев постранично.
    /// </summary>
    [HttpGet]
    [Route("pets/paged")]
    public async Task<IActionResult> GetPetsWithPagination(
        [FromQuery] GetPetsWithPaginationRequest request,
        [FromServices] IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationQuery> petsWithPaginationHandler,
        CancellationToken cancellationToken)
    {
        var result = await petsWithPaginationHandler.Handle(request.ToQuery(), cancellationToken);

        return Ok(Envelop.Ok(result));
    }
}

public record GetPetsWithPaginationRequest(
    int Page,
    int PageSize,
    Guid[]? VolunteerIds,
    string? NickName = null,
    string? Description = null,
    string? Color = null,
    string? HealthInformation = null,
    string? Address = null,
    double? Weight = null,
    double? Height = null,
    string? PhoneNumber = null,
    bool? IsCastrated = null,
    DateTime? DateOfBirth = null,
    bool? IsVaccinated = null)
{
    public GetPetsWithPaginationQuery ToQuery()
    {
        return new GetPetsWithPaginationQuery(
            VolunteerIds,
            NickName,
            Description,
            Color,
            HealthInformation,
            Address,
            Weight,
            Height,
            PhoneNumber,
            IsCastrated,
            DateOfBirth,
            IsVaccinated,
            Page,
            PageSize);
    }
}
