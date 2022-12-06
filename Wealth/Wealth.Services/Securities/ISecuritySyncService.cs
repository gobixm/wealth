namespace Wealth.Services.Securities;

public interface ISecuritySyncService
{
    Task SyncSecuritiesAsync(CancellationToken cancellationToken = default);
}