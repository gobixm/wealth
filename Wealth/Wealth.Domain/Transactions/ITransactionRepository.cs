namespace Wealth.Domain.Transactions;

public interface ITransactionRepository
{
    public Task<IReadOnlyCollection<Transaction>> GetAllAsync(int offset, int limit,
        CancellationToken cancellationToken = default);

    public Task<Transaction> AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
    public Task<long> DeleteAsync(IReadOnlyCollection<int> ids, CancellationToken cancellationToken = default);
}