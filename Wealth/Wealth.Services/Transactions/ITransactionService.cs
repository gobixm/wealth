using Wealth.Domain.Transactions;

namespace Wealth.Services.Transactions;

public interface ITransactionService
{
    Task<TransactionDto> AddTransactionAsync(AddTransactionRequest request,
        CancellationToken cancellationToken = default);
}