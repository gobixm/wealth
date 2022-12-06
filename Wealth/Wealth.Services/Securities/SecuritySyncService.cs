using Common.Collections;
using DataAccess.Abstractions;
using Moex.Abstractions;
using Wealth.Domain.Securities;

namespace Wealth.Services.Securities;

public sealed class SecuritySyncService : ISecuritySyncService
{
    private readonly IMoexApi _moexApi;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public SecuritySyncService(IUnitOfWorkFactory unitOfWorkFactory, IMoexApi moexApi)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _moexApi = moexApi;
    }

    public async Task SyncSecuritiesAsync(CancellationToken cancellationToken = default)
    {
        await using var unitOfWork = _unitOfWorkFactory.Create();
        var repository = await unitOfWork.GetRepositoryAsync<ISecurityRepository>(cancellationToken);

        var maxTerm = await repository.GetMaxTermAsync(cancellationToken) ?? 0;
        var nextTerm = maxTerm + 1;

        var moexSecurities = Pagination.PaginateAllAsync(async (start, limit, token) =>
            await _moexApi.GetSecuritiesAsync(start, limit, token), 100, cancellationToken);

        await foreach (var page in moexSecurities.WithCancellation(cancellationToken))
            await SyncPageAsync(page, repository, nextTerm, cancellationToken);

        await repository.SoftDeleteNotInTermAsync(nextTerm, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);
    }

    private async Task SyncPageAsync(IReadOnlyCollection<MoexSecurity> page, ISecurityRepository repository,
        int nextTerm, CancellationToken cancellationToken)
    {
        var moexIds = page.Select(x => x.Id).ToArray();
        var existingSecurities = (await repository.GetAsync(moexIds, cancellationToken)).ToDictionary(x => x.Id);
        var newSecurities = page.Where(x => !existingSecurities.ContainsKey(x.Id))
            .Select(newSecurity => new Security
            {
                Id = newSecurity.Id,
                Name = newSecurity.Name,
                Modified = DateTime.Now,
                Deleted = false,
                Term = nextTerm
            });
        await repository.AddAsync(newSecurities, cancellationToken);

        var updatedSecurities = page.Where(x => existingSecurities.ContainsKey(x.Id))
            .Select(security => existingSecurities[security.Id] with
            {
                Name = security.Name, Modified = DateTime.Now, Term = nextTerm, Deleted = false
            });

        await repository.UpdateAsync(updatedSecurities, cancellationToken);
    }
}