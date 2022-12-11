using DataAccess.Abstractions;
using Wealth.Domain.Securities;

namespace Wealth.Services.Securities;

public sealed class SecurityService : ISecurityService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public SecurityService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<IReadOnlyCollection<SecurityDto>> GetAsync(string? namePattern,
        int offset,
        int limit,
        CancellationToken cancellationToken = default)
    {
        await using var unitOfWork = _unitOfWorkFactory.Create();

        var result = await (await unitOfWork.GetRepositoryAsync<ISecurityRepository>(cancellationToken)).FindAsync(
            namePattern, offset,
            limit, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);
        return result.Select(x => new SecurityDto(x.Id, x.Name, x.Modified, x.Term, x.Deleted)).ToArray();
    }
}