using DataAccess.Abstractions;

namespace Wealth.Backend;

public sealed class MigratorHostedService : IHostedService
{
    private readonly IMigrator _migrator;

    public MigratorHostedService(IMigrator migrator)
    {
        _migrator = migrator;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _migrator.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}