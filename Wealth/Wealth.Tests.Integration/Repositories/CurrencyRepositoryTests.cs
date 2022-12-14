using AutoFixture;
using DataAccess.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Wealth.Domain.Currencies;

namespace Wealth.Tests.Integration.Repositories;

public sealed class CurrencyRepositoryTests : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;

    public CurrencyRepositoryTests(DbFixture fixture)
    {
        _fixture = fixture;
        _fixture.CleanupAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task InsertUpdateDelete_Success()
    {
        // arrange
        var currencies = new Fixture().Build<Currency>()
            .CreateMany(10)
            .ToList();
        await using var unitOfWork = _fixture.Provider.GetRequiredService<IUnitOfWorkFactory>().Create();
        var repo = await unitOfWork.GetRepositoryAsync<ICurrencyRepository>();

        // act
        var added = await repo.AddAsync(currencies);
        var afterAdd = (await repo.GetAsync()).Where(x => added.Select(c => c.Id).Contains(x.Id));
        var deleted = await repo.DeleteAsync(added.Take(5).Select(x => x.Id).ToList());
        var afterDelete = (await repo.GetAsync()).Where(x => added.Select(c => c.Id).Contains(x.Id));

        var update = added.Last() with
        {
            Name = "new name", Ticker = "USD"
        };
        var updated = await repo.UpdateAsync(new[] {update});
        var afterUpdate = await repo.GetAsync(update.Id);

        // assert
        added.Should().HaveCount(10);
        added.First().Should().BeEquivalentTo(currencies[0], options => options.Excluding(x => x.Id));
        afterAdd.Should().BeEquivalentTo(currencies, options => options.WithoutStrictOrdering().Excluding(x => x.Id));
        deleted.Should().Be(5);
        afterDelete.Should().BeEquivalentTo(currencies.Skip(5),
            options => options.WithoutStrictOrdering().Excluding(x => x.Id));
        updated.Should().Be(1);
        afterUpdate.Should().BeEquivalentTo(update);
    }
}