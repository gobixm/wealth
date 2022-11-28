using FluentMigrator;

namespace Wealth.Infrastructure.Migrations;

[Migration(20221128)]
public sealed class Initial_20221128 : Migration
{
    public override void Up()
    {
        Create.Table("securities")
            .WithColumn("Id").AsString(20).NotNullable().PrimaryKey()
            .WithColumn("Name").AsString(100);
    }

    public override void Down()
    {
        Delete.Table("securities");
    }
}