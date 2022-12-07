using Microsoft.AspNetCore.Mvc;
using Wealth.Services.Securities;

namespace Wealth.Backend.Controllers;

public sealed class SecuritiesController : ControllerBase
{
    public async Task SyncSecuritiesAsync([FromServices] SecuritySyncService service,
        CancellationToken cancellationToken)
    {
        await service.SyncSecuritiesAsync(cancellationToken);
    }
}