
using NovaUnlimited.Core.Interfaces.Events;
using Ardalis.GuardClauses;
using NovaUnlimited.Kernel;

namespace NovaUnlimited.Core.Entities.Events;

public class BaseEvent : BaseDomainEvent, ITurnEvent
{
    public int TurnNumber { get; private init; }

    public TurnPhase Phase { get; private init; }

    public bool IsProcessed { get; private set; }

    protected BaseEvent(int turnNumber, TurnPhase phase)
    {
        Guard.Against.NegativeOrZero(turnNumber, nameof(turnNumber));

        TurnNumber = turnNumber;
        Phase = phase;
    }

    public void MarkProcessed()
    {
        if (IsProcessed)
        {
            throw new InvalidOperationException("Event has already been processed.");
        }
        
        IsProcessed = true;
    }
}
