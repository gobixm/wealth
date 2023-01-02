namespace Wealth.Domain.Transactions;

public static class TransactionBuilder
{
    public static Transaction Build(AddTransactionRequest request)
    {
        var sumPrice = request.PricePerLot * request.Lots;
        var signedSumPrice = request.OperationType == OperationType.Buy ? -sumPrice : sumPrice;
        return new Transaction
        {
            Date = request.Date,
            Fee = request.Fee,
            Lots = request.Lots,
            CurrencyId = request.CurrencyId,
            OperationType = request.OperationType,
            SecurityId = request.SecurityId,
            PricePerLot = request.PricePerLot,
            TotalPrice = sumPrice,
            SignedSumPrice = signedSumPrice,
            SignedSumPriceFee = signedSumPrice - request.Fee
        };
    }
}