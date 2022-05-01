
using Ardalis.Specification;

namespace NovaUnlimited.Kernel.Interfaces;

public interface IRepository<T> :
    IRepositoryBase<T>
    where T : class, IAggregateRoot
{

}
