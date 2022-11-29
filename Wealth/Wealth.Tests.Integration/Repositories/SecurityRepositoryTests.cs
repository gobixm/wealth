using AutoFixture;
using DataAccess.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Wealth.Domain.Securities;

namespace Wealth.Tests.Integration.Repositories;

public sealed class SecurityRepositoryTests : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;

    public SecurityRepositoryTests(DbFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task InsertUpdateDelete_Success()
    {
        // arrange
        var securities = new Fixture().Build<Security>()
            .With(x => x.Modified, DateTime.Today)
            .CreateMany(10)
            .ToList();
        await using var unitOfWork = _fixture.Provider.GetRequiredService<IUnitOfWork>();
        var repo = await unitOfWork.GetRepositoryAsync<ISecurityRepository>();

        // act
        var added = await repo.AddAsync(securities);
        var afterAdd = await repo.GetAsync(securities.Select(x => x.Id).ToList());
        var deleted = await repo.DeleteAsync(securities.Take(5).Select(x => x.Id).ToList());
        var afterDelete = await repo.GetAsync(securities.Select(x => x.Id).ToList());

        var update = securities.Last() with {Name = "new name", Modified = DateTime.Today - TimeSpan.FromDays(1), Term = 42};
        var updated = await repo.UpdateAsync(new[] {update});
        var afterUpdate = await repo.GetAsync(new[] {update.Id});

        // assert
        added.Should().Be(10);
        afterAdd.Should().BeEquivalentTo(securities, options => options.WithoutStrictOrdering());
        deleted.Should().Be(5);
        afterDelete.Should().BeEquivalentTo(securities.Skip(5), options => options.WithoutStrictOrdering());
        updated.Should().Be(1);
        afterUpdate.Should().BeEquivalentTo(new[] {update});
    }
}