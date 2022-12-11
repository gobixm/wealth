using Wealth.Domain.Securities;

namespace Wealth.Services.Securities;

public interface ISecurityService
{
    Task<IReadOnlyCollection<SecurityDto>> GetAsync(string? namePattern,
        int offset,
        int limit,
        CancellationToken cancellationToken = default);
}