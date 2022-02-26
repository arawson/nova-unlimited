
using NovaUnlimited.Core.Util;
using Ardalis.GuardClauses;

namespace NovaUnlimited.Core.Entities.Events;

public class PlaceStarEvent : AbstractTileEvent
{
    public int Lifetime { get; private init; }

    public PlaceStarEvent(
        int turnNumber,
        TurnPhase phase,
        Hex location,
        int lifetime
    ) : base(turnNumber, phase, location) {
        Guard.Against.NegativeOrZero(lifetime, nameof(lifetime));

        Lifetime = lifetime;
    }
}
