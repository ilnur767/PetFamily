using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PetFamily.Application.Common;
using PetFamily.Infrastructure.Services;

namespace PetFamily.Infrastructure.BackgroundServices;

public class DeleteExpiredEntitiesService : BackgroundService
{
    private readonly int _schedulingInterval;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DeleteExpiredEntitiesService(IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<SchedulingOptions> schedulingOptions)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _schedulingInterval = schedulingOptions.CurrentValue.ScanFrequencyInHours;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested == false)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<DeleteExpiredVolunteersService>();

            await service.ExecuteDelete(cancellationToken);

            await Task.Delay(TimeSpan.FromHours(_schedulingInterval), cancellationToken);
        }
    }
}