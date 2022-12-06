namespace DataAccess.Abstractions;

public interface IUnitOfWorkFactory
{
    public IUnitOfWork Create();
}