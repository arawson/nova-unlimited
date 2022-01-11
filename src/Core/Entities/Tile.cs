
using NovaUnlimited.Core.Util;
using Ardalis.GuardClauses;

namespace NovaUnlimited.Core.Entities;

public enum TileType
{
    Empty,
    Star
}

public class Tile : BaseEntity
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
}
