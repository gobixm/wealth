using Dapper;
using DataAccess.Abstractions;
using DataAccess.Pg;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Wealth.Domain.Currencies;
using Wealth.Domain.Securities;
using Wealth.Infrastructure.Migrations;
using Wealth.Infrastructure.Repositories;

namespace Wealth.Tests.Integration;

public sealed class DbFixture : IAsyncLifetime
{
    private readonly PostgreSqlTestcontainer _dbContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "db",
            Password = "postgres",
            Username = "postgres",
        })
        .Build();

    public ServiceProvider Provider { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        var config = new Dictionary<string, string?>
        {
            {"Pg:ConnectionString", _dbContainer.ConnectionString}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddPgSql(new PgRepositoryFactoryOptions()
            .RegisterRepository<ISecurityRepository, SecurityRepository>()
            .RegisterRepository<ICurrencyRepository, CurrencyRepository>(), typeof(Initial_20221128).Assembly);
        serviceCollection.AddTransient<IConfiguration>(_ => configuration);

        Provider = serviceCollection.BuildServiceProvider();

        var migrator = Provider.GetRequiredService<IMigrator>();
        await migrator.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    public async Task CleanupAsync()
    {
        await using var connection = new NpgsqlConnection(_dbContainer.ConnectionString);
        await connection.ExecuteAsync("delete from securities");
    }
}