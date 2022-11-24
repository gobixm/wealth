using Npgsql;

namespace DataAccess.Pg;

public class PgRepositoryFactory
{
    private readonly PgRepositoryFactoryOptions _options;

    public PgRepositoryFactory(PgRepositoryFactoryOptions options)
    {
        _options = options;
    }

    public T Create<T>(NpgsqlConnection connection)
    {
        var repoType = _options.GetRepositoryType<T>();
        if (repoType is null) throw new ArgumentException();

        // todo: optimize
        return (T) Activator.CreateInstance(repoType, connection)!;
    }
}