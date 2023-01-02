using Microsoft.AspNetCore.Mvc;
using Wealth.Domain.Transactions;
using Wealth.Services.Transactions;

namespace Wealth.Backend.Controllers;

[Route("transactions")]
public sealed class TransactionsController : ControllerBase
{
    [HttpPost]
    public async Task<TransactionDto> AddTransactionAsync(AddTransactionRequest request,
        [FromServices] ITransactionService transactionService, CancellationToken cancellationToken)
    {
        return await transactionService.AddTransactionAsync(request, cancellationToken);
    }
}