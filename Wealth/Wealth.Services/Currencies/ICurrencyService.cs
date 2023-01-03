using Wealth.Domain.Currencies;

namespace Wealth.Services.Currencies;

public interface ICurrencyService
{
    Task<IReadOnlyCollection<CurrencyDto>> GetAsync(CancellationToken cancellationToken = default);
}