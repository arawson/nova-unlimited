
using NovaUnlimited.Core.Entities.Events;

namespace NovaUnlimited.Core.Interfaces.Events;

public interface IEventDispatcher
{
    void QueueEvent(AbstractEvent e);

    int TurnNumber { get; }
}
