using DataAccess.Abstractions;
using Wealth.Domain.Currencies;

namespace Wealth.Services.Currencies;

public sealed class CurrencyService : ICurrencyService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public CurrencyService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<IReadOnlyCollection<CurrencyDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = await unitOfWork.GetRepositoryAsync<ICurrencyRepository>(cancellationToken);

        var currencies = await repository.GetAsync(cancellationToken);

        return currencies.Select(x => new CurrencyDto(x.Id, x.Ticker, x.Name)).ToArray();
    }
}