using Common.Collections;
using FluentAssertions;

namespace Common.Tests.Collections;

public sealed class PaginationTests
{
    [Fact]
    public async Task PaginateAllFlat_All_Returned()
    {
        // arrange
        var source = Enumerable.Range(1, 10).ToArray();

        // act
        var all = await Pagination.PaginateAllFlatAsync((start, limit, _) =>
                Task.FromResult((IReadOnlyCollection<int>) source.Skip(start).Take(limit).ToArray()), 3)
            .ToListAsync();

        // assert
        all.Should().BeEquivalentTo(source, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task PaginateAll_Pages_Returned()
    {
        // arrange
        var source = Enumerable.Range(1, 10).ToArray();

        // act
        var all = await Pagination.PaginateAllAsync((start, limit, _) =>
                Task.FromResult((IReadOnlyCollection<int>) source.Skip(start).Take(limit).ToArray()), 3)
            .ToListAsync();

        // assert
        var expected = new List<IReadOnlyCollection<int>>
        {
            source.Skip(0).Take(3).ToArray(),
            source.Skip(3).Take(3).ToArray(),
            source.Skip(6).Take(3).ToArray(),
            source.Skip(9).Take(3).ToArray(),
        };
        all.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }
}