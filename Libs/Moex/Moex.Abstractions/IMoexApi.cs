namespace Moex.Abstractions;

public interface IMoexApi : IDisposable
{
    Task<IReadOnlyCollection<Security>> GetSecuritiesAsync(int start, int limit,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<IndexSecurity>> GetIndexSecuritiesAsync(string index, int start, int limit,
        CancellationToken cancellationToken = default);
}