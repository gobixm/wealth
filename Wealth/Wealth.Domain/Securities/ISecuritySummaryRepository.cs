namespace Wealth.Domain.Securities;

public interface ISecuritySummaryRepository
{
    public Task<SecuritySummary> UpsertAsync(SecuritySummaryChange securitySummaryChange,
        CancellationToken cancellationToken = default);

    public Task<IReadOnlyCollection<SecuritySummary>> GetAsync(ICollection<string> secIds,
        CancellationToken cancellationToken = default);
}