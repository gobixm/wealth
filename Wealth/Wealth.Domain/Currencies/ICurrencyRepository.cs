namespace Wealth.Domain.Currencies;

public interface ICurrencyRepository
{
    public Task<IReadOnlyCollection<Currency>> GetAsync(CancellationToken cancellationToken = default);
    public Task<Currency?> GetAsync(int id, CancellationToken cancellationToken = default);
    public Task<long> DeleteAsync(ICollection<int> ids, CancellationToken cancellationToken = default);
    public Task<long> UpdateAsync(IEnumerable<Currency> securities, CancellationToken cancellationToken = default);
    public Task<long> AddAsync(IEnumerable<Currency> securities, CancellationToken cancellationToken = default);
}