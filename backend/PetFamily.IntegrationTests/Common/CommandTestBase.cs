using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;

namespace PetFamily.IntegrationTests.Common;

public abstract class CommandTestBase<TResponse, TCommand> : TestBase
    where TCommand : ICommand
{
    protected readonly ICommandHandler<TResponse, TCommand> Sut;

    protected CommandTestBase(IntegrationTestsWebFactory factory) : base(factory)
    {
        Sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<TResponse, TCommand>>();
    }
}
