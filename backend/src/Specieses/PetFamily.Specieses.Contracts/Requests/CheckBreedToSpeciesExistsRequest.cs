namespace PetFamily.Specieses.Contracts.Requests;

public record CheckBreedToSpeciesExistsRequest(Guid SpeciesId, Guid BreedId);
