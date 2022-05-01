using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NovaUnlimited.Kernel.Interfaces;

namespace NovaUnlimited.Infrastructure.Data;

public class EfRepository<T> :
    RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public EfRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
