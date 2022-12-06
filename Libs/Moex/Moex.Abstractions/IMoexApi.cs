namespace Moex.Abstractions;

public interface IMoexApi : IDisposable
{
    Task<IReadOnlyCollection<MoexSecurity>> GetSecuritiesAsync(int start, int limit,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<IndexSecurity>> GetIndexSecuritiesAsync(string index, int start, int limit,
        CancellationToken cancellationToken = default);
}