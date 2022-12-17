namespace Wealth.Domain.Currencies;

public interface ICurrencyRepository
{
    public Task<IReadOnlyCollection<Currency>> GetAsync(CancellationToken cancellationToken = default);
    public Task<Currency?> GetAsync(int id, CancellationToken cancellationToken = default);
    public Task<long> DeleteAsync(ICollection<int> ids, CancellationToken cancellationToken = default);
    public Task<long> UpdateAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default);

    public Task<IReadOnlyCollection<Currency>> AddAsync(IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default);
}