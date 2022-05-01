
using Ardalis.Specification;

namespace NovaUnlimited.Kernel.Interfaces;

public interface IReadRepository<T> :
    IReadRepositoryBase<T>
    where T : class, IAggregateRoot
{
    
}
