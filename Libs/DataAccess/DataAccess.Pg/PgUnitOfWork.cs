using DataAccess.Abstractions;
using Npgsql;

namespace DataAccess.Pg;

public sealed class PgUnitOfWork : IUnitOfWork
{
    private readonly PgRepositoryFactory _repositoryFactory;
    private readonly PgOptions _options;
    private NpgsqlConnection? _connection;
    private NpgsqlTransaction? _transaction;

    public PgUnitOfWork(PgOptions options, PgRepositoryFactory repositoryFactory)
    {
        _options = options;
        _repositoryFactory = repositoryFactory;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
            await _transaction.DisposeAsync();
        if (_connection is not null)
            await _connection.DisposeAsync();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;

        await _transaction.CommitAsync(cancellationToken);
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;

        await _transaction.RollbackAsync(cancellationToken);
        _transaction = null;
    }

    public async Task<T> GetRepositoryAsync<T>(CancellationToken cancellationToken = default) where T : class
    {
        if (_connection is null)
        {
            _connection = new NpgsqlConnection(_options.ConnectionString);
            await _connection.OpenAsync(cancellationToken);
            _transaction = await _connection.BeginTransactionAsync(cancellationToken);
        }

        return _repositoryFactory.Create<T>(_connection);
    }
}