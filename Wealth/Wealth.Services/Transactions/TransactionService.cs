using DataAccess.Abstractions;
using Wealth.Domain.Securities;
using Wealth.Domain.Transactions;

namespace Wealth.Services.Transactions;

public sealed class TransactionService : ITransactionService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public TransactionService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<TransactionDto> AddTransactionAsync(AddTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var transactionRepository = await unitOfWork.GetRepositoryAsync<ITransactionRepository>(cancellationToken);
        var secsumRepository = await unitOfWork.GetRepositoryAsync<ISecuritySummaryRepository>(cancellationToken);

        var transaction = TransactionBuilder.Build(request);

        var saved = await transactionRepository.AddAsync(transaction, cancellationToken);
        await secsumRepository.UpsertAsync(GetSummaryChange(saved), cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);

        return new TransactionDto
        {
            Id = saved.Id,
            Date = saved.Date,
            Fee = saved.Fee,
            Lots = saved.Lots,
            CurrencyId = saved.CurrencyId,
            OperationType = saved.OperationType,
            SecurityId = saved.SecurityId,
            TotalPrice = saved.TotalPrice,
            PricePerLot = saved.PricePerLot,
            SignedSumPrice = saved.SignedSumPrice,
            SignedSumPriceFee = saved.SignedSumPriceFee
        };
    }

    private static SecuritySummaryChange GetSummaryChange(Transaction saved)
    {
        return new SecuritySummaryChange
        {
            Id = saved.SecurityId,
            Lots = saved.OperationType == OperationType.Buy ? saved.Lots : -saved.Lots,
            TotalPrice = saved.OperationType == OperationType.Buy ? saved.TotalPrice : -saved.TotalPrice,
            TotalFee = -saved.Fee,
            TotalPriceWithFee = saved.OperationType == OperationType.Buy
                ? saved.TotalPrice
                : -saved.TotalPrice - saved.Fee
        };
    }
}