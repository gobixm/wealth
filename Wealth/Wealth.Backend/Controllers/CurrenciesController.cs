using Microsoft.AspNetCore.Mvc;
using Wealth.Domain.Currencies;
using Wealth.Services.Currencies;

namespace Wealth.Backend.Controllers;

[Route("currencies")]
public sealed class CurrenciesController : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyCollection<CurrencyDto>> GetAsync([FromServices] ICurrencyService currencyService,
        CancellationToken cancellationToken)
    {
        return await currencyService.GetAsync(cancellationToken);
    }
}