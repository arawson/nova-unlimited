
using NovaUnlimited.Core.Entities.Events;

namespace NovaUnlimited.Core.Interfaces.Events;

public interface IEventContainer
{
    void QueueEvent(BaseEvent e);

    int TurnNumber { get; }
}
