
using MediatR;
using NovaUnlimited.Core.Interfaces;

namespace NovaUnlimited.Kernel;

public abstract class BaseDomainEvent : INotification, IIdentifiable
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;

    public long Id { get; protected set; }
}
