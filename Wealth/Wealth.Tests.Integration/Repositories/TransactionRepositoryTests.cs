using AutoFixture;
using DataAccess.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Wealth.Domain.Currencies;
using Wealth.Domain.Securities;
using Wealth.Domain.Transactions;

namespace Wealth.Tests.Integration.Repositories;

public sealed class TransactionRepositoryTests : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;

    public TransactionRepositoryTests(DbFixture fixture)
    {
        _fixture = fixture;
        _fixture.CleanupAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task InsertUpdateDelete_Success()
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

        var transaction = new Fixture().Build<Transaction>()
            .With(x => x.CurrencyId, currency.Id)
            .With(x => x.SecurityId, security.Id)
            .With(x => x.Date, DateTime.Today)
            .Create();

        var repo = await unitOfWork.GetRepositoryAsync<ITransactionRepository>();

        // act
        var savedTransaction = await repo.AddAsync(transaction);
        var retrievedTransaction = await repo.GetAllAsync(0, 1);
        await repo.DeleteAsync(new[] {savedTransaction.Id});
        var afterDelete = await repo.GetAllAsync(0, 1);

        // assert
        savedTransaction.Should().BeEquivalentTo(transaction, options => options.Excluding(x => x.Id));
        retrievedTransaction.Should().BeEquivalentTo(new[] {savedTransaction});
        afterDelete.Should().BeEmpty();
    }
}