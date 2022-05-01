
using Ardalis.GuardClauses;
using NovaUnlimited.Core.Entities.Events;
using NovaUnlimited.Core.Interfaces.Events;
using NovaUnlimited.Kernel;
using NovaUnlimited.Kernel.Interfaces;

namespace NovaUnlimited.Core.Entities;

public class Game : BaseEntity, IEventContainer, IAggregateRoot
{
    public int RuleSetVersion { get; set; }

    public int TurnNumber { get; set; }

    public Map Map { get; set; }

    public List<BaseEvent> Events { get; set; }

    public override List<BaseDomainEvent> DomainEvents { get; set; }

    public Game()
    {
        // constructor for efcore
        Events = new List<BaseEvent>();
        DomainEvents = new List<BaseDomainEvent>();

        Map = new Map(1);
    }

    public Game(
        int ruleSetVersion,
        Map map
    )
    {
        Guard.Against.Null(map, nameof(map));
        Map = map;

        Guard.Against.NegativeOrZero(ruleSetVersion, nameof(ruleSetVersion));
        RuleSetVersion = ruleSetVersion;

        TurnNumber = 1;
        
        Events = new List<BaseEvent>();
        DomainEvents = new List<BaseDomainEvent>();
    }

    public void IncrementTurn()
    {
        TurnNumber++;
    }

    public void QueueEvent(BaseEvent e)
    {
        Guard.Against.AgainstExpression(
            t => t >= TurnNumber,
            e.TurnNumber,
            "Cannot queue an event for a previous turn."
        );

        Guard.Against.AgainstExpression(
            tup => !(tup.TurnNumber == TurnNumber && tup.Phase == TurnPhase.PreTurn),
            (e.TurnNumber, e.Phase),
            "Cannot queue a PreTurn event during the same turn."
        );

        if (e is ITileEvent)
        {
            var te = e as ITileEvent;
            if (!Map.IsWithin(te!.Location))
                throw new IndexOutOfRangeException("Tile Event outside of map.");
        }

        /*var elist = Events.GetValueOrDefault(e.TurnNumber);
        if (elist == null)
        {
            elist = new List<AbstractEvent>();
            Events[e.TurnNumber] = elist;
        }
        */

        Events.Add(e);
    }
    
}
