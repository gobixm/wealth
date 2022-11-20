namespace DataAccess.Pg;

public sealed class PgRepositoryFactoryOptions
{
    private readonly Dictionary<Type, Type> _mappings = new();

    public PgRepositoryFactoryOptions RegisterRepository<T, TImp>()
    {
        _mappings.Add(typeof(T), typeof(TImp));
        return this;
    }

    public Type? GetRepositoryType<T>()
    {
        return _mappings.TryGetValue(typeof(T), out var type) ? type : null;
    }
}