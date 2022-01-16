
using NovaUnlimited.Core.Entities;

namespace NovaUnlimited.Core.Interfaces.Events;

public interface ITurnEvent
{
    int TurnNumber { get; }
    TurnPhase Phase { get; }
}
