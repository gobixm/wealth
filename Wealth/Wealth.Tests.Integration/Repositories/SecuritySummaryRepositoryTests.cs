using AutoFixture;
using DataAccess.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Wealth.Domain.Currencies;
using Wealth.Domain.Securities;

namespace Wealth.Tests.Integration.Repositories;

public sealed class SecuritySummaryRepositoryTests : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;

    public SecuritySummaryRepositoryTests(DbFixture fixture)
    {
        _fixture = fixture;
        _fixture.CleanupAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task InsertUpdateGet_Success()
    {
        // arrange
        var currency = new Fixture().Build<Currency>().Create();
        var security = new Fixture().Build<Security>().Create();
        await using var unitOfWork = _fixture.Provider.GetRequiredService<IUnitOfWorkFactory>().Create();
        await (await unitOfWork.GetRepositoryAsync<ISecurityRepository>())
            .AddAsync(new[] {security});

        currency = (await (await unitOfWork.GetRepositoryAsync<ICurrencyRepository>())
                .AddAsync(new[] {currency}))
            .Single();

        var change1 = new SecuritySummaryChange
        {
            Id = security.Id,
            Lots = 10,
            CurrencyId = currency.Id,
            TotalFee = 42.3M,
            TotalPrice = 1000M,
            TotalPriceWithFee = 1042.3M
        };

        var change2 = new SecuritySummaryChange
        {
            Id = security.Id,
            Lots = -10,
            CurrencyId = currency.Id,
            TotalFee = 42.3M,
            TotalPrice = -1000M,
            TotalPriceWithFee = -1042.3M
        };

        var repo = await unitOfWork.GetRepositoryAsync<ISecuritySummaryRepository>();

        // act
        var inserted = await repo.UpsertAsync(change1);
        var retrieved = await repo.GetAsync(new[] {security.Id});
        var updated = await repo.UpsertAsync(change2);
        var retrievedAfterUpdate = await repo.GetAsync(new[] {security.Id});

        // assert
        inserted.Should().BeEquivalentTo(change1);
        retrieved.Should().BeEquivalentTo(new[] {inserted});
        updated.Should().BeEquivalentTo(new SecuritySummary
        {
            Id = security.Id,
            Lots = 0,
            CurrencyId = currency.Id,
            TotalFee = 84.6M,
            TotalPrice = 0,
            TotalPriceWithFee = 0
        });
        retrievedAfterUpdate.Should().BeEquivalentTo(new[] {updated});
    }
}