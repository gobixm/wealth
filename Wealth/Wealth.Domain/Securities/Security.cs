namespace Wealth.Domain.Securities;

public sealed record Security
{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public DateTime Modified { get; init; }
    public int Term { get; init; }
    
    public bool Deleted { get; init; }
}