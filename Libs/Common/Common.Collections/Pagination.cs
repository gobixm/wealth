using System.Runtime.CompilerServices;

namespace Common.Collections;

public static class Pagination
{
    public static async IAsyncEnumerable<T> PaginateAllFlatAsync<T>(
        Func<int, int, CancellationToken, Task<IReadOnlyCollection<T>>> query,
        int pageSize = 100, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var result = PaginateAllAsync(
            async (start, limit, token) => await query(start, limit, token), pageSize,
            cancellationToken);

        await foreach (var page in result.WithCancellation(cancellationToken))
        foreach (var item in page)
            yield return item;
    }

    public static async IAsyncEnumerable<IReadOnlyCollection<T>> PaginateAllAsync<T>(
        Func<int, int, CancellationToken, Task<IReadOnlyCollection<T>>> query,
        int pageSize = 100,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var start = 0;
        while (true)
        {
            var page = await query(start, pageSize, cancellationToken);
            yield return page;
            if (page.Count < pageSize) break;

            start += page.Count;
        }
    }
}