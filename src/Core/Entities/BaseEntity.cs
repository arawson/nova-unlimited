
using NovaUnlimited.Core.Interfaces;
using NovaUnlimited.Kernel;

namespace NovaUnlimited.Core.Entities;

public abstract class BaseEntity : IIdentifiable
{
    public long Id { get; set; }

    abstract public List<BaseDomainEvent> DomainEvents { get; set; }
}
