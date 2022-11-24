using Npgsql;

namespace DataAccess.Pg;

public abstract class PgRepository
{
    protected PgRepository(NpgsqlConnection connection)
    {
        Connection = connection;
    }

    protected NpgsqlConnection Connection { get; }
}