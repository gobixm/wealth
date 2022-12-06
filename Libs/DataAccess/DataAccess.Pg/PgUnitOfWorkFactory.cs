using DataAccess.Abstractions;

namespace DataAccess.Pg;

public sealed class PgUnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly PgOptions _pgOptions;
    private readonly PgRepositoryFactory _pgRepositoryFactory;

    public PgUnitOfWorkFactory(PgOptions pgOptions, PgRepositoryFactoryOptions repositoryFactoryOptions)
    {
        _pgOptions = pgOptions;
        _pgRepositoryFactory = new PgRepositoryFactory(repositoryFactoryOptions);
    }

    public IUnitOfWork Create()
    {
        return new PgUnitOfWork(_pgOptions, _pgRepositoryFactory);
    }
}