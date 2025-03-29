using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Files.AddFile;
using PetFamily.Application.Files.DeleteFile;
using PetFamily.Application.Files.GetFileLink;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.AddPetPhoto;
using PetFamily.Application.Volunteers.ChangePetPosition;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.DeletePetPhoto;
using PetFamily.Application.Volunteers.HardDelete;
using PetFamily.Application.Volunteers.Restore;
using PetFamily.Application.Volunteers.SoftDelete;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialMedias;

namespace PetFamily.Application.Inject;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateRequisitesHandler>();
        services.AddScoped<UpdateSocialMediasHandler>();
        services.AddScoped<HardDeleteVolunteerHandler>();
        services.AddScoped<SoftDeleteVolunteerHandler>();
        services.AddScoped<RestoreVolunteerHandler>();
        services.AddScoped<AddFileHandler>();
        services.AddScoped<GetFileLinkHandler>();
        services.AddScoped<DeleteFileHandler>();
        services.AddScoped<AddPetCommandHandler>();
        services.AddScoped<AddPetPhotoCommandHandler>();
        services.AddScoped<DeletePetPhotoCommandHandler>();
        services.AddScoped<ChangePetPositionHandler>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}
