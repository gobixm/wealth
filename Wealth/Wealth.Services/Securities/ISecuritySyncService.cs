using Wealth.Domain.Securities;

namespace Wealth.Services.Securities;

public interface ISecuritySyncService
{
    Task<SecuritySyncProgressDto> SyncSecuritiesAsync(CancellationToken cancellationToken = default);
    SecuritySyncProgressDto GetSyncProgress(Guid id);
}