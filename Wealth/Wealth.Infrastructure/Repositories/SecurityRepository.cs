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
                "update securities set Name=@Name, Modified=@Modified, Term=@Term, Deleted=@Deleted where Id=@Id",
                security,
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
                "insert into securities (Id, Name, Modified, Term, Deleted) values (@Id, @Name, @Modified, @Term, @Deleted)",
                security,
                cancellationToken: cancellationToken));
        }

        return counter;
    }

    public async Task<int?> GetMaxTermAsync(CancellationToken cancellationToken = default)
    {
        return await Connection.QuerySingleOrDefaultAsync<int?>(new CommandDefinition("select max(Term) from securities",
            cancellationToken: cancellationToken));
    }

    public async Task<int> SoftDeleteNotInTermAsync(int term, CancellationToken cancellationToken = default)
    {
        return await Connection.ExecuteAsync(new CommandDefinition(
            "update securities set Deleted=@Deleted, Term=@Term, Modified=@Modified where Term <> @Term",
            new {Deleted = true, Term = term, Modified = DateTime.Now}, cancellationToken: cancellationToken));
    }
}