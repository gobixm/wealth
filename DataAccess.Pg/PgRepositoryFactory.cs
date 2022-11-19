using Npgsql;

namespace DataAccess.Pg;

public class PgRepositoryFactory
{
    public T Create<T>(NpgsqlConnection connection)
    {
        // todo: optimize
        if (!typeof(PgRepository).IsAssignableFrom(typeof(T))) throw new InvalidOperationException();

        // todo: optimize
        return (T) Activator.CreateInstance(typeof(T), connection)!;
    }
}