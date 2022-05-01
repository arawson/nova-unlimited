
using Ardalis.GuardClauses;
using NovaUnlimited.Core.Entities;
using NovaUnlimited.Core.Entities.Events;
using NovaUnlimited.Core.Interfaces;
using NovaUnlimited.Core.Interfaces.Events;
using NovaUnlimited.Core.Interfaces.RuleSets;

namespace NovaUnlimited.Core.RuleSets.V1;

// instead of being an entity, a game ruleset has to be reconstructed from
// a map on load

/*
Here's an idea to make this modular: replace direct event handling with a set
of pre-built handlers
PlaceStarEvent, for example, can be encapsulated into a single handler
GameStartEvent also has a similar problem where it could be just one function
    for a long span of time

The most important factor here is that the changes _MUST_ make their way back to
the Game object, since the Game object is the thing which is persisted to the
database.
*/

public class NovaUnlimitedV1 : IGameRuleSetV1
{
    public int Version => 1;

    public int TurnNumber => Game.TurnNumber;

    private SortedDictionary<int, List<BaseEvent>> Events;

    private Game Game;

    private PlaceStarEventHandlerV1 placeStarEventHandlerV1;

    public NovaUnlimitedV1(Game game)
    {
        Guard.Against.Null(game, nameof(game));
        Game = game;

        Events = new SortedDictionary<int, List<BaseEvent>>();

        foreach (var e in Game.Events.Where(x => x.TurnNumber > Game.TurnNumber))
        {
            TrackQueueEvent(e);
        }

        placeStarEventHandlerV1 = new PlaceStarEventHandlerV1(Game, this);
    }

    public List<BaseEvent> ExecutePostTurn(int turnNumber)
    {
        throw new NotImplementedException();
    }

    public List<BaseEvent> ExecutePreTurn(int turnNumber)
    {
        throw new NotImplementedException();
    }

#region "Event Handling"
    public void QueueEvent(BaseEvent e)
    {
        Game.QueueEvent(e);
        
        // Game is already throwing the exceptions for us

        TrackQueueEvent(e);
    }

    private void TrackQueueEvent(BaseEvent e)
    {
        List<BaseEvent>? eList = null;
        if (Events.TryGetValue(e.TurnNumber, out eList)) {
            eList.Add(e);
        } else {
            eList = new List<BaseEvent>();
            eList.Add(e);
            Events[e.TurnNumber] = eList;
        }
    }

    private void ProcessPreTurnEvents()
    {
        var elist = Events.GetValueOrDefault(Game.TurnNumber);
        if (elist == null) return; // EZPZ

        var ptevents = elist.Where(x => x.Phase == TurnPhase.PreTurn);
        foreach (var e in ptevents)
        {
            ProcessBaseEvent(e);
        }
    }

    private void ProcessPostTurnEvents()
    {
        var elist = Events.GetValueOrDefault(Game.TurnNumber);
        if (elist == null) return; // EZPZ

        var ptevents = elist.Where(x => x.Phase == TurnPhase.PostTurn);
        foreach (var e in ptevents)
        {
            ProcessBaseEvent(e);
        }
    }

    private void ProcessBaseEvent(BaseEvent e)
    {
        // TODO See Tile for the note on handling using a type-table
        var tileEvent = e as ITileEvent;
        if (tileEvent == null)
        {
            // Handle map-wide events
        }
        else
        {
            // Handle Tile events
            var tile = Game.Map.TileAt(tileEvent.Location);
            // this one shouldn't happen since we guard on the insert side
            Guard.Against.Null(tile, nameof(tile), "Tile not found.");
            // TODO: tile.Handle(tileEvent);
        }

        e.MarkProcessed();
    }
#endregion "Event Handling"

#region "Turn Handling"
    public void PreTurn()
    {
        ProcessPreTurnEvents();
    }

    public void PostTurn()
    {
        ProcessPostTurnEvents();

        Game.IncrementTurn();
    }

    #endregion
}
