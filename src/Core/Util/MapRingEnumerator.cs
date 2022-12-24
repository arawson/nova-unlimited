


using System.Collections;
using NovaUnlimited.Core.Entities;

namespace NovaUnlimited.Core.Util;

public class MapRingEnumerator : IEnumerator<Tile?>
{
    private const HexDirection START_DIRECTION = HexDirection.SouthEast;

    public Map Map { get; private init; }
    public Tile? Current { get; private set; }

    object IEnumerator.Current => Current!;

    private Hex Position;
    private Hex RingLast;
    private HexDirection Direction = START_DIRECTION;
    private bool InCenter = true;

    private readonly Hex LastJump = new Hex(1,-2);

    public MapRingEnumerator(Map m)
    {
        Map = m;
        // Really fancy iteration from the middle.
        // It might be easier than iterating around like a square though.
        Position = Map.Center;
    }

    public void Dispose() {}

    public bool MoveNext()
    {

        if (Map.IsWithin(Position)) {
            Current = Map.TileAt(Position);
        } else {
            return false;
        }

        // If we're in the center we setup the ring loops
        if (InCenter) {
            Position = Position.Neighbor(HexDirection.North);
            RingLast = Position.Neighbor(HexDirection.SouthWest);
            InCenter = false;
            return true;
        }

        if (Position == RingLast)
        {
            // Reset the ring loop when we would otherwise move to the top
            // position on the ring
            Position += LastJump;
            RingLast = Position.Neighbor(HexDirection.SouthWest);
            Direction = START_DIRECTION;
        } else {
            // Advance by one for the next iteration
            Position += Hex.GetVector(Direction);
            
            // We know we're on an axis if S, Q, or R are 0 NOPE!
            if (Position.R == Map.Radius
                || Position.Q == Map.Radius
                || Position.R + Position.Q == 2 * Map.Radius)
            {
                Direction = HexHelper.RotateCW(Direction);
            }
        }

        return true;
    }

    public void Reset()
    {
        Position = Map.Center;
        Current = Map.TileAt(Position)!;
        InCenter = true;
        Direction = START_DIRECTION;
    }
}
