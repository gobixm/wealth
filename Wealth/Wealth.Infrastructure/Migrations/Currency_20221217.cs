using FluentMigrator;
using FluentMigrator.Postgres;

namespace Wealth.Infrastructure.Migrations;

[Migration(20221217)]
public sealed class Currency_20221217 : Migration
{
    public override void Up()
    {
        Create.Table("currencies")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("ticker").AsString()
            .WithColumn("name").AsString();

        Create.Index("idx_currencies_ticker")
            .OnTable("currencies")
            .OnColumn("ticker")
            .Ascending();

        Insert.IntoTable("currencies").Row(new {id = 1, ticker = "RUB", name = "Russian ruble"});
    }

    public override void Down()
    {
        Delete.Table("currencies");
    }
}