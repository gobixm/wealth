using Dapper;
using DataAccess.Pg;
using Npgsql;
using Wealth.Domain.Securities;

namespace Wealth.Infrastructure.Repositories;

public sealed class SecurityRepository : PgRepository, ISecurityRepository
{
    public SecurityRepository(NpgsqlConnection connection) : base(connection)
    {
    }

    public async Task<IReadOnlyCollection<Security>> GetAsync(ICollection<string> ids,
        CancellationToken cancellationToken = default)
    {
        var result =
            await Connection.QueryAsync<Security>(new CommandDefinition("select * from securities where Id = any(@ids)",
                new {ids}, cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<long> DeleteAsync(ICollection<string> ids, CancellationToken cancellationToken = default)
    {
        return await Connection.ExecuteAsync(new CommandDefinition("delete from securities where Id = any(@ids)",
            new {ids},
            cancellationToken: cancellationToken));
    }

    public async Task<long> UpdateAsync(IEnumerable<Security> securities, CancellationToken cancellationToken = default)
    {
        var counter = 0;
        foreach (var security in securities)
        {
            cancellationToken.ThrowIfCancellationRequested();

            counter += await Connection.ExecuteAsync(new CommandDefinition(
                "update securities set Name=@Name, Modified=@Modified, Term=@Term where Id=@Id", security,
                cancellationToken: cancellationToken));
        }

        return counter;
    }

    public async Task<long> AddAsync(IEnumerable<Security> securities, CancellationToken cancellationToken = default)
    {
        var counter = 0;
        foreach (var security in securities)
        {
            cancellationToken.ThrowIfCancellationRequested();

            counter += await Connection.ExecuteAsync(new CommandDefinition(
                "insert into securities (Id, Name, Modified, Term) values (@Id, @Name, @Modified, @Term)", security,
                cancellationToken: cancellationToken));
        }

        return counter;
    }
}