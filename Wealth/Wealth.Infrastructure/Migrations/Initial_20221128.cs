using FluentMigrator;

namespace Wealth.Infrastructure.Migrations;

[Migration(20221128)]
public sealed class Initial_20221128 : Migration
{
    public override void Up()
    {
        Create.Table("securities")
            .WithColumn("id").AsString(100).NotNullable().PrimaryKey()
            .WithColumn("name").AsString()
            .WithColumn("modified").AsDateTime2().NotNullable()
            .WithColumn("term").AsInt32().NotNullable()
            .WithColumn("deleted").AsBoolean().NotNullable();

        Create.Index("idx_securities_deleted")
            .OnTable("securities")
            .OnColumn("deleted")
            .Ascending();

        Create.Index("idx_securities_name")
            .OnTable("securities")
            .OnColumn("name")
            .Ascending();
    }

    public override void Down()
    {
        Delete.Table("securities");
    }
}