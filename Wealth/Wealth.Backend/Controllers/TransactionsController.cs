using Microsoft.AspNetCore.Mvc;
using Wealth.Domain.Transactions;
using Wealth.Services.Transactions;

namespace Wealth.Backend.Controllers;

[ApiController]
[Route("transactions")]
public sealed class TransactionsController : ControllerBase
{
    [HttpPost]
    public async Task<TransactionDto> AddTransactionAsync([FromBody] AddTransactionRequest request,
        [FromServices] ITransactionService transactionService, CancellationToken cancellationToken)
    {
        return await transactionService.AddTransactionAsync(request, cancellationToken);
    }
}