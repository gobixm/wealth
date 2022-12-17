using Dapper;
using DataAccess.Pg;
using Npgsql;
using Wealth.Domain.Currencies;

namespace Wealth.Infrastructure.Repositories;

public sealed class CurrencyRepository : PgRepository, ICurrencyRepository
{
    public CurrencyRepository(NpgsqlConnection connection) : base(connection)
    {
    }

    public async Task<IReadOnlyCollection<Currency>> GetAsync(CancellationToken cancellationToken = default)
    {
        return (await Connection.QueryAsync<Currency>(new CommandDefinition("select * from currencies",
            cancellationToken: cancellationToken))).ToArray();
    }

    public async Task<Currency?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Connection.QueryFirstOrDefaultAsync<Currency>(new CommandDefinition(
            "select * from currencies where Id=@Id", new {Id = id},
            cancellationToken: cancellationToken));
    }

    public async Task<long> DeleteAsync(ICollection<int> ids, CancellationToken cancellationToken = default)
    {
        return await Connection.ExecuteAsync(new CommandDefinition("delete from currencies where Id = any(@ids)",
            new {ids},
            cancellationToken: cancellationToken));
    }

    public async Task<long> UpdateAsync(IEnumerable<Currency> securities, CancellationToken cancellationToken = default)
    {
        var counter = 0;
        foreach (var security in securities)
        {
            cancellationToken.ThrowIfCancellationRequested();

            counter += await Connection.ExecuteAsync(new CommandDefinition(
                "update currencies set Name=@Name, Ticker=@Ticker where Id=@Id",
                security,
                cancellationToken: cancellationToken));
        }

        return counter;
    }

    public async Task<long> AddAsync(IEnumerable<Currency> securities, CancellationToken cancellationToken = default)
    {
        var counter = 0;
        foreach (var security in securities)
        {
            cancellationToken.ThrowIfCancellationRequested();

            counter += await Connection.ExecuteAsync(new CommandDefinition(
                "insert into currencies (Id, Ticker, Name) values (@Id, @Ticker, @Name)",
                security,
                cancellationToken: cancellationToken));
        }

        return counter;
    }
}