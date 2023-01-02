namespace Wealth.Domain.Securities;

public class SecuritySummaryChange
{
    public string Id { get; set; } = null!;
    public int Lots { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TotalFee { get; init; }
    public decimal TotalPriceWithFee { get; init; }

    public int CurrencyId { get; set; }
}