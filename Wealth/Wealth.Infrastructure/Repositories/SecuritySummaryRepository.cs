using Dapper;
using DataAccess.Pg;
using Npgsql;
using Wealth.Domain.Securities;

namespace Wealth.Infrastructure.Repositories;

public sealed class SecuritySummaryRepository : PgRepository, ISecuritySummaryRepository
{
    public SecuritySummaryRepository(NpgsqlConnection connection) : base(connection)
    {
    }

    public async Task<SecuritySummary> UpsertAsync(SecuritySummaryChange securitySummaryChange,
        CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            @"
insert into secsum (Id, Lots, TotalPrice, TotalFee, TotalPriceWithFee, CurrencyId)
values (@Id, @Lots, @TotalPrice, @TotalFee, @TotalPriceWithFee, @CurrencyId)
on conflict (id)
do update set Lots = secsum.Lots + EXCLUDED.Lots,
    TotalPrice = secsum.TotalPrice + EXCLUDED.TotalPrice,
    TotalFee = secsum.TotalFee + EXCLUDED.TotalFee,
    TotalPriceWithFee = secsum.TotalPriceWithFee + EXCLUDED.TotalPriceWithFee
returning *", securitySummaryChange, cancellationToken: cancellationToken);

        return await Connection.QuerySingleAsync<SecuritySummary>(command);
    }

    public async Task<IReadOnlyCollection<SecuritySummary>> GetAsync(ICollection<string> secIds,
        CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            @"select * from secsum where Id = any(@ids)", new {ids = secIds}, cancellationToken: cancellationToken);

        return (await Connection.QueryAsync<SecuritySummary>(command)).ToList();
    }
}