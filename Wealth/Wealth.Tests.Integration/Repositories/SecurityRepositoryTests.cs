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
        _fixture.CleanupAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task InsertUpdateDelete_Success()
    {
        // arrange
        var securities = new Fixture().Build<Security>()
            .With(x => x.Modified, DateTime.Today)
            .CreateMany(10)
            .ToList();
        await using var unitOfWork = _fixture.Provider.GetRequiredService<IUnitOfWorkFactory>().Create();
        var repo = await unitOfWork.GetRepositoryAsync<ISecurityRepository>();

        // act
        var added = await repo.AddAsync(securities);
        var afterAdd = await repo.GetAsync(securities.Select(x => x.Id).ToList());
        var deleted = await repo.DeleteAsync(securities.Take(5).Select(x => x.Id).ToList());
        var afterDelete = await repo.GetAsync(securities.Select(x => x.Id).ToList());

        var update = securities.Last() with
        {
            Name = "new name", Modified = DateTime.Today - TimeSpan.FromDays(1), Term = 42
        };
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

    [Fact]
    public async Task SoftDeleteNotInTermAsync_Success()
    {
        // arrange
        var securitiesTerm2 = new Fixture().Build<Security>()
            .With(x => x.Deleted, false)
            .With(x => x.Term, () => 2)
            .CreateMany(2)
            .ToList();

        var securitiesTerm3 = new Fixture().Build<Security>()
            .With(x => x.Deleted, false)
            .With(x => x.Term, () => 3)
            .CreateMany(2)
            .ToList();

        await using var unitOfWork = _fixture.Provider.GetRequiredService<IUnitOfWorkFactory>().Create();
        var repo = await unitOfWork.GetRepositoryAsync<ISecurityRepository>();
        await repo.AddAsync(securitiesTerm2.Concat(securitiesTerm3));

        // act
        await repo.SoftDeleteNotInTermAsync(3);

        // assert
        var securities2 = await repo.GetAsync(securitiesTerm2.Select(x => x.Id).ToArray());
        securities2.Should().AllSatisfy(x => x.Term.Should().Be(3));
        securities2.Should().AllSatisfy(x => x.Deleted.Should().Be(true));

        var securities3 = await repo.GetAsync(securitiesTerm3.Select(x => x.Id).ToArray());
        securities3.Should().AllSatisfy(x => x.Deleted.Should().Be(false));
    }

    [Fact]
    public async Task Find_Securities_Returned()
    {
        // arrange
        var securities = new Fixture().Build<Security>()
            .With(x => x.Modified, DateTime.Today)
            .CreateMany(10)
            .ToArray();

        for (var i = 0; i < securities.Length; i++) securities[i] = securities[i] with {Name = $"foo{i}"};

        await using var unitOfWork = _fixture.Provider.GetRequiredService<IUnitOfWorkFactory>().Create();
        var repo = await unitOfWork.GetRepositoryAsync<ISecurityRepository>();
        await repo.AddAsync(securities);

        // act
        var all = await repo.FindAsync(null, 0, 10);
        var allByPattern = await repo.FindAsync("foo", 0, 10);
        var one = await repo.FindAsync("foo1", 0, 10);
        var page = await repo.FindAsync("foo", 3, 3);

        // assert
        all.Should().BeEquivalentTo(securities);
        allByPattern.Should().BeEquivalentTo(securities);
        one.Should().BeEquivalentTo(new[] {securities[1]});
        page.Should().BeEquivalentTo(securities.Skip(3).Take(3));
    }

    [Fact]
    public async Task GetMaxTerm_Term_Returned()
    {
        // arrange
        var securitiesTerm2 = new Fixture().Build<Security>()
            .With(x => x.Modified, DateTime.Today)
            .With(x => x.Term, () => 2)
            .CreateMany(2)
            .ToList();

        var securitiesTerm3 = new Fixture().Build<Security>()
            .With(x => x.Modified, DateTime.Today)
            .With(x => x.Term, () => 3)
            .CreateMany(2)
            .ToList();

        await using var unitOfWork = _fixture.Provider.GetRequiredService<IUnitOfWorkFactory>().Create();
        var repo = await unitOfWork.GetRepositoryAsync<ISecurityRepository>();

        // act
        var noTerm = await repo.GetMaxTermAsync();
        await repo.AddAsync(securitiesTerm2.Concat(securitiesTerm3));
        var maxTerm = await repo.GetMaxTermAsync();

        // assert
        noTerm.Should().BeNull();
        maxTerm.Should().Be(3);
    }
}