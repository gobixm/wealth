using Dapper;
using DataAccess.Pg;
using Npgsql;
using Wealth.Domain.Transactions;

namespace Wealth.Infrastructure.Repositories;

public class TransactionRepository : PgRepository, ITransactionRepository
{
    public TransactionRepository(NpgsqlConnection connection) : base(connection)
    {
    }

    public async Task<IReadOnlyCollection<Transaction>> GetAllAsync(int offset, int limit,
        CancellationToken cancellationToken = default)
    {
        const string query = "select * from transactions order by Date desc limit @Limit offset @Offset";

        return (await Connection.QueryAsync<Transaction>(new CommandDefinition(
                query, new {Limit = limit, Offset = offset},
                cancellationToken: cancellationToken)))
            .ToArray();
    }

    public async Task<Transaction> AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        const string query =
            @"insert into transactions (SecurityId, OperationType, Date, Lots, CurrencyId, PricePerLot, TotalPrice, SignedSumPrice, Fee, SignedSumPriceFee)
                             values (@SecurityId, @OperationType, @Date, @Lots, @CurrencyId, @PricePerLot, @TotalPrice, @SignedSumPrice, @Fee, @SignedSumPriceFee) 
                             returning *";

        return await Connection.QuerySingleAsync<Transaction>(new CommandDefinition(query, transaction,
            cancellationToken: cancellationToken));
    }

    public async Task<long> DeleteAsync(IReadOnlyCollection<int> ids, CancellationToken cancellationToken = default)
    {
        return await Connection.ExecuteAsync(new CommandDefinition("delete from transactions where Id = any(@ids)",
            new {ids},
            cancellationToken: cancellationToken));
    }
}