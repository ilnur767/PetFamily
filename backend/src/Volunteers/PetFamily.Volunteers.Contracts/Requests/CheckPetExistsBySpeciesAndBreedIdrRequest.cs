namespace PetFamily.Volunteers.Contracts.Requests;

public record CheckPetExistsBySpeciesAndBreedIdRequest(Guid SpesiesId, Guid BreedId);

public record CheckPetExistsBySpeciesIdRequest(Guid SpesiesId);
