namespace Wealth.Domain.Transactions;

public sealed record AddTransactionRequest
{
    public string SecurityId { get; init; } = null!;
    public OperationType OperationType { get; init; }
    public DateTime Date { get; init; }
    public int Lots { get; init; }
    public int CurrencyId { get; init; }

    public decimal PricePerLot { get; init; }
    public decimal Fee { get; init; }
}