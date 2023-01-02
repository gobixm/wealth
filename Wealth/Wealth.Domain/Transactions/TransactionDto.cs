namespace Wealth.Domain.Transactions;

public sealed record TransactionDto
{
    public int Id { get; init; }
    public string SecurityId { get; init; } = null!;
    public OperationType OperationType { get; init; }
    public DateTime Date { get; init; }
    public int Lots { get; init; }
    public int CurrencyId { get; init; }

    public decimal PricePerLot { get; init; }
    public decimal TotalPrice { get; init; }
    public decimal SignedSumPrice { get; init; }
    public decimal Fee { get; init; }
    public decimal SignedSumPriceFee { get; init; }
}