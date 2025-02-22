using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Application.Common;

namespace PetFamily.Infrastructure.Services;

public sealed class DeleteExpiredVolunteersService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly int _expirationDays;
    private readonly ILogger<DeleteExpiredVolunteersService> _logger;
    private readonly TimeProvider _timeProvider;

    public DeleteExpiredVolunteersService(ApplicationDbContext dbContext, TimeProvider timeProvider,
        IOptionsMonitor<SchedulingOptions> options, ILogger<DeleteExpiredVolunteersService> logger)
    {
        _dbContext = dbContext;
        _timeProvider = timeProvider;
        _expirationDays = options.CurrentValue.DeleteVolunteersExpirationDays;
        _logger = logger;
    }

    public async Task ExecuteDelete(CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        sw.Start();

        var currentDateTime = _timeProvider.GetUtcNow().UtcDateTime;
        var dateToDelete = currentDateTime.AddDays(-_expirationDays);

        var volunteersToDelete = await _dbContext.Volunteers
            .Where(v => v.IsDeleted == true && v.DeletedAt <= dateToDelete).ToListAsync(cancellationToken);
        _dbContext.Volunteers.RemoveRange(volunteersToDelete);

        await _dbContext.SaveChangesAsync(cancellationToken);
        sw.Stop();

        _logger.LogInformation(
            "The process of deleting volunteers is completed in {sw}. Deleted volunteers count: {count}.",
            sw.Elapsed, volunteersToDelete.Count());
    }
}