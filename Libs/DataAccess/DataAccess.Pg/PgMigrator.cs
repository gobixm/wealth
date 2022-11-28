using DataAccess.Abstractions;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Pg;

public sealed class PgMigrator : IMigrator
{
    private readonly PgOptions _options;
    private readonly IServiceProvider _serviceProvider;

    public PgMigrator(PgOptions options, IServiceProvider serviceProvider)
    {
        _options = options;
        _serviceProvider = serviceProvider;
    }

    public Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();

        return Task.CompletedTask;
    }
}