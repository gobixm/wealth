using DataAccess.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Pg;

public static class PgExtensions
{
    public static IServiceCollection AddPgSql(this IServiceCollection serviceCollection, PgRepositoryFactoryOptions repositoryFactoryOptions)
    {
        serviceCollection.AddSingleton(x =>
            x.GetRequiredService<IConfiguration>().GetSection("Pg")
                .Get<PgOptions>());

        serviceCollection.AddTransient<IUnitOfWork>(sp =>
            new PgUnitOfWork(sp.GetRequiredService<PgOptions>(), new PgRepositoryFactory(repositoryFactoryOptions)));

        return serviceCollection;
    }
}