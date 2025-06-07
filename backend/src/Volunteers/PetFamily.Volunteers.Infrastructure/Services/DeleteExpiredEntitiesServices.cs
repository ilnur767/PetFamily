using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PetFamily.Volunteers.Application.Common;

namespace PetFamily.Volunteers.Infrastructure.Services;

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
        await Task.Delay(50000);

        while (cancellationToken.IsCancellationRequested == false)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<DeleteExpiredVolunteersService>();

            await service.ExecuteDelete(cancellationToken);

            await Task.Delay(TimeSpan.FromHours(_schedulingInterval), cancellationToken);
        }
    }
}
