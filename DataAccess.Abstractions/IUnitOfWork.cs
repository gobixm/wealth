namespace DataAccess.Abstractions;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    public Task CommitAsync(CancellationToken cancellationToken = default);
    public Task RollbackAsync(CancellationToken cancellationToken = default);
    public Task<T> GetRepositoryAsync<T>(CancellationToken cancellationToken = default) where T : class;
}