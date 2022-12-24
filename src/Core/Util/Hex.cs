
namespace NovaUnlimited.Core.Util;

/// Logic derived from
/// https://www.redblobgames.com/grids/hexagons/
/// Flat Topped, Odd Q

public enum HexDirection : int
{
    SouthEast, NorthEast, North,
    NorthWest, SouthWest, South
}

public static class HexHelper
{
    public static HexDirection RotateCW(HexDirection d)
    {
        switch (d) {
            case HexDirection.North: return HexDirection.NorthEast;
            case HexDirection.NorthEast: return HexDirection.SouthEast;
            case HexDirection.SouthEast: return HexDirection.South;
            case HexDirection.South: return HexDirection.SouthWest;
            case HexDirection.SouthWest: return HexDirection.NorthWest;
            case HexDirection.NorthWest: return HexDirection.North;
            default: return HexDirection.North;
        }
    }

    public static HexDirection RotateCCW(HexDirection d)
    {
        switch (d) {
            case HexDirection.North: return HexDirection.NorthWest;
            case HexDirection.NorthWest: return HexDirection.SouthWest;
            case HexDirection.SouthWest: return HexDirection.South;
            case HexDirection.South: return HexDirection.SouthEast;
            case HexDirection.SouthEast: return HexDirection.NorthEast;
            case HexDirection.NorthEast: return HexDirection.North;
            default: return HexDirection.North;
        }
    }
}

public readonly record struct Hex
{
    public static Hex[] DirectionVectors =
    {
        new(+1, 0), new(+1, -1), new(0, -1),
        new(-1, 0), new(-1, +1), new(0, +1),
    };

    public static Hex GetVector(HexDirection d)
    {
        return DirectionVectors[(int)d];
    }

    public int Q { get; private init; }
    public int R { get; private init; }
    public int S { get => -Q - R; }

    public Hex(int q, int r)
    {
        Q = q;
        R = r;
    }

    public static Hex operator +(Hex a, Hex b)
    => new(
        a.Q + b.Q,
        a.R + b.R
    );

    public static Hex operator -(Hex a, Hex b)
    => new(
        a.Q - b.Q,
        a.R - b.R
    );

    public Hex Neighbor(HexDirection d)
    => this + GetVector(d);

    public static int Distance(Hex a, Hex b)
    => (
        Math.Abs(a.Q - b.Q)
        + Math.Abs(a.Q + a.R - b.Q - b.R)
        + Math.Abs(a.R - b.R)
    ) / 2;

    public int Distance(Hex b) => Distance (this, b);
}
