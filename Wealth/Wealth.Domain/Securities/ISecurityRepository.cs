namespace Wealth.Domain.Securities;

public interface ISecurityRepository
{
    public Task<IReadOnlyCollection<Security>> GetAsync(ICollection<string> ids,
        CancellationToken cancellationToken = default);
    public Task<long> DeleteAsync(ICollection<string> ids, CancellationToken cancellationToken = default);
    public Task<long> UpdateAsync(IEnumerable<Security> securities, CancellationToken cancellationToken = default);
    public Task<long> AddAsync(IEnumerable<Security> securities, CancellationToken cancellationToken = default);
}