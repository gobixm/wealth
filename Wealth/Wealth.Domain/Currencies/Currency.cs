namespace Wealth.Domain.Currencies;

public sealed record Currency
{
    public int Id { get; init; }
    public string Ticker { get; init; } = null!;
    public string Name { get; init; } = null!;
}