
using NovaUnlimited.Core.Util;
using Ardalis.GuardClauses;
using NovaUnlimited.Core.Exceptions;

namespace NovaUnlimited.Core.Entities;

/*
The hexagonal storage of the entire universe!
*/
public class Map : BaseEntity
{
    public Tile[,] Tiles { get; private set; }
    public Hex Size { get; private set; }
    public int Radius { get; private set; }
    public int Diameter { get; private set; }

    public Map(int radius)
    {
        Guard.Against.NegativeOrZero(radius, nameof(radius));

        Radius = radius;
        // Diameter will always be odd, can't make a hexagon
        // with an even diameter in this system
        Diameter = radius * 2 + 1;

        Size = new(Diameter, Diameter);
        Tiles = new Tile[Diameter, Diameter];
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
}
