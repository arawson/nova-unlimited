
using NovaUnlimited.Core.Util;
using NovaUnlimited.Core.Interfaces.Events;
using Ardalis.GuardClauses;
using NovaUnlimited.Core.Entities.Events;

namespace NovaUnlimited.Core.Entities;

public enum TileType
{
    Empty,
    Star
}

public class Tile : BaseEntity, ITileEventListener, IPlaceStarEventListener
{
    public TileType Contents { get; private set; }
    public Star? Star { get; private set; }

    /// Initialize an empty tile
    public Tile(Map map, Hex location)
    {
        Guard.Against.Null(map, nameof(map));

        Contents = TileType.Empty;
        Star = null;
    }

    public void Handle(ITileEvent tileEvent)
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

    public void Handle(PlaceStarEvent e)
    {
        throw new NotImplementedException();
    }
}
