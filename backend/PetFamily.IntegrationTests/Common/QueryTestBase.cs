using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;

namespace PetFamily.IntegrationTests.Common;

public abstract class QueryTestBase<TResponse, TQuery> : TestBase
    where TQuery : IQuery
{
    protected readonly IQueryHandler<TResponse, TQuery> Sut;

    protected QueryTestBase(IntegrationTestsWebFactory factory) : base(factory)
    {
        Sut = Scope.ServiceProvider.GetRequiredService<IQueryHandler<TResponse, TQuery>>();
    }
}
