using FluentMigrator;
using FluentMigrator.Postgres;

namespace Wealth.Infrastructure.Migrations;

[Migration(20221217)]
public sealed class CurrencyTransaction_20221217 : Migration
{
    public override void Up()
    {
        Create.Table("currencies")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("ticker").AsString().NotNullable()
            .WithColumn("name").AsString().NotNullable();

        Create.Index("idx_currencies_ticker")
            .OnTable("currencies")
            .OnColumn("ticker")
            .Ascending();

        Create.Table("transactions")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("securityid").AsString().ForeignKey("securities", "id").NotNullable()
            .WithColumn("operationtype").AsInt16().NotNullable()
            .WithColumn("date").AsDateTime2().NotNullable()
            .WithColumn("lots").AsInt32().NotNullable()
            .WithColumn("currencyid").AsInt32().ForeignKey("currencies", "id").NotNullable()
            .WithColumn("priceperlot").AsDecimal().NotNullable()
            .WithColumn("totalprice").AsDecimal().NotNullable()
            .WithColumn("signedsumprice").AsDecimal().NotNullable()
            .WithColumn("fee").AsDecimal().NotNullable()
            .WithColumn("signedsumpricefee").AsDecimal().NotNullable();

        Insert.IntoTable("currencies").Row(new {ticker = "RUB", name = "Russian ruble"});
    }

    public override void Down()
    {
        Delete.Table("currencies");
    }
}