using Microsoft.AspNetCore.Mvc;
using Wealth.Domain.Securities;
using Wealth.Services.Securities;

namespace Wealth.Backend.Controllers;

[Route("secs")]
public sealed class SecuritiesController : ControllerBase
{
    [HttpPost("sync")]
    public async Task<SecuritySyncProgressDto> SyncSecuritiesAsync([FromServices] ISecuritySyncService service,
        CancellationToken cancellationToken)
    {
        return await service.SyncSecuritiesAsync(cancellationToken);
    }


    [HttpGet("sync-progress/{id:guid}")]
    public SecuritySyncProgressDto GetProgress(Guid id, [FromServices] ISecuritySyncService service)
    {
        return service.GetSyncProgress(id);
    }
}