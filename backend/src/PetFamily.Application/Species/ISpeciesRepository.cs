using CSharpFunctionalExtensions;
using PetFamily.Domain.Common;

namespace PetFamily.Application.Species;

public interface ISpeciesRepository
{
    public Task<Result<Domain.Specieses.Species, Error>> GetById(Guid id, CancellationToken cancellationToken);
}
