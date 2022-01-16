
using NovaUnlimited.Core.Util;
using Ardalis.GuardClauses;
using NovaUnlimited.Core.Entities.Events;
using NovaUnlimited.Core.Interfaces.Events;
using NovaUnlimited.Core.Interfaces;
using NovaUnlimited.Core.Exceptions;

namespace NovaUnlimited.Core.Entities;

/*
The hexagonal storage of the entire universe!
*/
public class Map : BaseEntity, IEventDispatcher, ITurnable
{
    public Tile[,] Tiles { get; private set; }
    public Hex Size { get; private set; }
    public int Radius { get; private set; }
    public int Diameter { get; private set; }
    public int TurnNumber { get; private set; }

    protected SortedDictionary<int, List<AbstractEvent>> Events { get; private set; }
    
    public Map(int radius)
    {
        Guard.Against.NegativeOrZero(radius, nameof(radius));

        Radius = radius;
        // Diameter will always be odd, can't make a hexagon
        // with an even diameter in this system
        Diameter = radius * 2 + 1;

        Size = new(Diameter, Diameter);
        Tiles = new Tile[Diameter, Diameter];

        TurnNumber = 1;

        Events = new SortedDictionary<int, List<AbstractEvent>>();
    }

#region "Helper Methods"
    
#endregion "Helper Methods"

#region "Map/Tile Manipulation"
    public void PutTile(Tile tile, Hex pos)
    {
        Guard.Against.Null(tile, nameof(tile));
        if (TileAt(pos) == tile) return;

        if (!this.IsWithin(pos))
            throw new IndexOutOfRangeException("Location " + pos + " outside of map boundaries.");

        if (Tiles[pos.Q, pos.R] != null)
            throw new TileAlreadyInitializedException("Tile already assigned at " + pos);
        
        Tiles[pos.Q, pos.R] = tile;
    }

    public Tile? TileAt(Hex pos)
    {
        return TileAt(pos.Q, pos.R);
    }

    public Tile? TileAt(int q, int r)
    {
        Tile ?t = null;
        if (IsWithin(q,r)) t = Tiles[q, r];
        return t;
    }

    public bool IsWithin(Hex pos)
    {
        return IsWithin(pos.Q, pos.R);
    }

    public bool IsWithin(int q, int r)
    {
        return
            (q >= 0)
            && (r >= 0)
            && (q <= Tiles.GetUpperBound(0))
            && (q <= Tiles.GetUpperBound(1))
            && (q >= Radius - r)
            && (q <= Radius * 3 - r);
            /* Deal with the triangular regions at the top.
            They might look something like this
            
            O for empty, 1 for populated/allowable
            7x7 (radius = 3)
            R   \/ center column always filled
            0 000 1 111
            1 001 1 111
            2 011 1 111
            3 111 1 111 - center row always filled
            4 111 1 110
            5 111 1 100
            6 111 1 000

              012 3 456 (Q)

            how bout one hex? 3x3
            011
            111
            110

            above, Q is the column and R is the row
            array lookup would be Tile[loc.Q, loc.R]

            the excluded triangles are radius in length
            q >= radius - r
            what is the output of this function?
            000 1 111 radius - r = 3
            001 1 111 = 2
            011 1 111 = 1
            111 1 111 = 0
            ...
            that solves the top triangle
            
            now for the bottom triangle
            q <= radius * 3 - r
            111 1 111 radius * 3 - r = 9
            111 1 111 = 8
            111 1 111 = 7
            111 1 111 = 6
            111 1 110 = 5
            111 1 100 = 4
            111 1 000 = 3

            check on radius 1
            radius * 3 = 3
            111 3
            111 2
            110 1
            */
    }
#endregion "Map/Tile Manipulation"

#region "Event Handling"
    public void QueueEvent(AbstractEvent e)
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
            if (!IsWithin(te!.Location))
                throw new IndexOutOfRangeException("Tile Event outside of map.");
        }

        var elist = Events.GetValueOrDefault(e.TurnNumber);
        if (elist == null)
        {
            elist = new List<AbstractEvent>();
            Events[e.TurnNumber] = elist;
        }

        elist.Add(e);
    }
    
    private void ProcessPreTurnEvents()
    {
        var elist = Events.GetValueOrDefault(TurnNumber);
        if (elist == null) return; // EZPZ

        var ptevents = elist.Where(x => x.Phase == TurnPhase.PreTurn);
        foreach (var e in ptevents)
        {
            ProcessBaseEvent(e);
        }
    }

    private void ProcessPostTurnEvents()
    {
        var elist = Events.GetValueOrDefault(TurnNumber);
        if (elist == null) return; // EZPZ

        var ptevents = elist.Where(x => x.Phase == TurnPhase.PostTurn);
        foreach (var e in ptevents)
        {
            ProcessBaseEvent(e);
        }
    }

    private void ProcessBaseEvent(AbstractEvent e)
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
            var tile = TileAt(tileEvent.Location);
            // this one shouldn't happen since we guard on the insert side
            Guard.Against.Null(tile, nameof(tile), "Tile not found.");
            tile.Handle(tileEvent);
        }

        e.MarkProcessed();
    }

    #endregion "Event Manipulation"

#region "Turn Handling"
    public void PreTurn()
    {
        ProcessPreTurnEvents();
    }

    public void PostTurn()
    {
        ProcessPostTurnEvents();

        TurnNumber++;
    }
#endregion

}
