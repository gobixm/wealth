using FluentMigrator;

namespace Wealth.Infrastructure.Migrations;

[Migration(20221128)]
public sealed class Initial_20221128 : Migration
{
    public override void Up()
    {
        Create.Table("securities")
            .WithColumn("id").AsString(100).NotNullable().PrimaryKey()
            .WithColumn("name").AsString(100)
            .WithColumn("modified").AsDateTime2().NotNullable()
            .WithColumn("term").AsInt32().NotNullable()
            .WithColumn("deleted").AsBoolean().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("securities");
    }
}