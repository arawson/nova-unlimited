
using NovaUnlimited.Core.Util;
using NovaUnlimited.Core.Interfaces.Events;
using Ardalis.GuardClauses;
using NovaUnlimited.Core.Entities.Events;
using NovaUnlimited.Core.Interfaces;

namespace NovaUnlimited.Core.Entities;

public enum TileType
{
    Empty,
    Star
}

public class Tile : IIdentifiable
{
    public long Id { get; set; }
    public TileType Contents { get; set; }
    public Star? Star { get; set; }

    /// Initialize an empty tile
    public Tile(Hex location)
    {
        Contents = TileType.Empty;
        Star = null;
    }

    public Tile(Map m, Hex location) : this(location) {}

    public Tile() {}

    //[Deprecated]
    private void Handle(ITileEvent tileEvent)
    {
        /*
        TODO search through the child game-objects and dispatch events
        to them based on their implementations of interfaces
        ideally, there would be a static type lookup table which maps the
        listener interfaces to the different event types
        that table would be populated at startup (privately, via reflection)
        and some attributes

        for now, this is just going to switch on type
        */
        Guard.Against.Null(tileEvent, nameof(tileEvent));

        switch(tileEvent)
        {
            case PlaceStarEvent pse:
                this.Handle(pse);
                break;
            
            default:
                throw new NotImplementedException("Handling not implemented for "
                    + tileEvent.GetType().ToString());
        }

        // what post processing does a tile need to do?
        // the map handles marking events as processed
    }

    // deprecated
    private void Handle(PlaceStarEvent e)
    {
        throw new NotImplementedException();
    }
}
