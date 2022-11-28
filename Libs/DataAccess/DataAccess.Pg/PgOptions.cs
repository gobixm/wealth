namespace DataAccess.Pg;

public sealed record PgOptions
{
    public string ConnectionString { get; init; } = null!;
}