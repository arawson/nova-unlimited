
using NovaUnlimited.Core.Interfaces.Events;
using Ardalis.GuardClauses;
using NovaUnlimited.Core.Util;

namespace NovaUnlimited.Core.Entities.Events;

public abstract class AbstractTileEvent : BaseEvent, ITileEvent
{
    public Hex Location { get; private init; }

    protected AbstractTileEvent(int turnNumber, TurnPhase phase, Hex location)
    : base(turnNumber, phase)
    {
        Location = location;
    }
}
