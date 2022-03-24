
using NovaUnlimited.Core.Entities.Events;

namespace NovaUnlimited.Core.Interfaces.Events;

public interface IEventContainer
{
    void QueueEvent(AbstractEvent e);

    int TurnNumber { get; }
}
