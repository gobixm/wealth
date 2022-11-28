using System.Reflection;
using DataAccess.Abstractions;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Pg;

public static class PgExtensions
{
    public static IServiceCollection AddPgSql(this IServiceCollection serviceCollection,
        PgRepositoryFactoryOptions repositoryFactoryOptions, Assembly migrations)
    {
        serviceCollection.AddSingleton(x =>
            x.GetRequiredService<IConfiguration>().GetSection("Pg")
                .Get<PgOptions>());

        serviceCollection.AddTransient<IUnitOfWork>(sp =>
            new PgUnitOfWork(sp.GetRequiredService<PgOptions>(), new PgRepositoryFactory(repositoryFactoryOptions)));

        serviceCollection.AddTransient<IMigrator, PgMigrator>();

        serviceCollection.AddFluentMigratorCore()
            .ConfigureRunner(x => x.AddPostgres()
                .WithGlobalConnectionString(sp => sp.GetRequiredService<PgOptions>().ConnectionString)
                .ScanIn(migrations)
            );

        return serviceCollection;
    }
}