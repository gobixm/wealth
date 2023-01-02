using FluentMigrator;

namespace Wealth.Infrastructure.Migrations;

[Migration(20230102)]
public sealed class SecuritySummary_20230102 : Migration
{
    public override void Up()
    {
        Create.Table("secsum")
            .WithColumn("id").AsString(100).NotNullable().PrimaryKey()
            .WithColumn("lots").AsInt32().NotNullable()
            .WithColumn("totalprice").AsDecimal().NotNullable()
            .WithColumn("totalfee").AsDecimal().NotNullable()
            .WithColumn("totalpricewithfee").AsDecimal().NotNullable()
            .WithColumn("currencyid").AsInt32().ForeignKey("currencies", "id").NotNullable();
    }

    public override void Down()
    {
        Delete.Table("secsum");
    }
}