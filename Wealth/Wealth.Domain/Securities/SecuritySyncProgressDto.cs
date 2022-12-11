namespace Wealth.Domain.Securities;

public record SecuritySyncProgressDto(Guid Id, bool Completed)
{
    public int Count { get; set; }
}