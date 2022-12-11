using System.Collections.Concurrent;
using Common.Collections;
using DataAccess.Abstractions;
using Moex.Abstractions;
using Wealth.Domain.Securities;

namespace Wealth.Services.Securities;

public sealed class SecuritySyncService : ISecuritySyncService
{
    private readonly IMoexApi _moexApi;
    private readonly ConcurrentDictionary<Guid, SecuritySyncProgressDto> _runningSyncs = new();
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public SecuritySyncService(IUnitOfWorkFactory unitOfWorkFactory, IMoexApi moexApi)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _moexApi = moexApi;
    }

    public Task<SecuritySyncProgressDto> SyncSecuritiesAsync(CancellationToken cancellationToken = default)
    {
        var progress = new SecuritySyncProgressDto(Guid.NewGuid(), false);
        _runningSyncs.TryAdd(progress.Id, progress);
        _ = SyncAsync(progress, cancellationToken);
        return Task.FromResult(progress);
    }

    public SecuritySyncProgressDto GetSyncProgress(Guid id)
    {
        return _runningSyncs.TryGetValue(id, out var progress) ? progress : new SecuritySyncProgressDto(id, true);
    }

    private async Task SyncAsync(SecuritySyncProgressDto progress, CancellationToken cancellationToken)
    {
        try
        {
            await using var unitOfWork = _unitOfWorkFactory.Create();
            var repository = await unitOfWork.GetRepositoryAsync<ISecurityRepository>(cancellationToken);

            var maxTerm = await repository.GetMaxTermAsync(cancellationToken) ?? 0;
            var nextTerm = maxTerm + 1;

            var moexSecurities = Pagination.PaginateAllAsync(async (start, limit, token) =>
                await _moexApi.GetSecuritiesAsync(start, limit, token), 100, cancellationToken);

            await foreach (var page in moexSecurities.WithCancellation(cancellationToken))
                await SyncPageAsync(page, repository, nextTerm, progress, cancellationToken);

            await repository.SoftDeleteNotInTermAsync(nextTerm, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
        }
        finally
        {
            _runningSyncs.TryRemove(progress.Id, out _);
        }
    }

    private async Task SyncPageAsync(IReadOnlyCollection<MoexSecurity> page, ISecurityRepository repository,
        int nextTerm, SecuritySyncProgressDto progress, CancellationToken cancellationToken)
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

        progress.Count += page.Count;
    }
}