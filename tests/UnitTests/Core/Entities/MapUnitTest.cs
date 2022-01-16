
namespace NovaUnlimited.Core.Entities;

using System;
using NovaUnlimited.Core.Entities.Events;
using NovaUnlimited.Core.Exceptions;
using NovaUnlimited.Core.Util;
using Xunit;

public class MapUnitTest
{
    public const int TestMapRadius = 13;

    public static Map GenMap()
    {
        return new Map(TestMapRadius);
    }

    [Fact]
    public void TestMapPreConditions()
    {
        var map = GenMap();

        Assert.Equal(1, map.TurnNumber);
        Assert.Equal(TestMapRadius, map.Radius);
    }

    [Fact]
    public void TestMapDimensions()
    {
        // Given
        var map = GenMap();
        // When
    
        // Then
        Assert.Equal(27, map.Diameter);
        Assert.Equal(TestMapRadius, map.Radius);
        Assert.Equal(27, map.Size.Q);
        Assert.Equal(27, map.Size.R);
    }

    [Theory]
    [InlineData(13,13,true)]
    [InlineData(5,8,true)]
    [InlineData(13,0,true)]
    [InlineData(0,13,true)]
    [InlineData(25,14,true)]
    [InlineData(26,13,true)]
    [InlineData(13,26,true)]
    [InlineData(0,0,false)]
    [InlineData(26,26,false)]
    [InlineData(4,8,false)]
    [InlineData(5,7,false)]
    [InlineData(26,14,false)]
    [InlineData(25,15,false)]
    [InlineData(-1,-1,false)]
    [InlineData(100,100,false)]
    public void TestMapBounds(int q, int r, bool inside)
    {
        var map = GenMap();
        Hex pos = new(q, r);

        if (inside)
            Assert.True(map.IsWithin(pos));
        else
            Assert.False(map.IsWithin(pos));
    }

    [Theory]
    [InlineData(0,0)]
    [InlineData(27,27)]
    [InlineData(-1,-1)]
    public void TestMapPutTileOutside(int q, int r)
    {
        var map = GenMap();
        Hex loc = new(q,r);
        var tile = new Tile(map, loc);

        Assert.Throws<IndexOutOfRangeException>(() =>
            {
                map.PutTile(tile, loc);
            }
        );
    }

    [Theory]
    [InlineData(13, 13)]
    public void TestMapPutTileOnBlank(int q, int r)
    {
        var map = GenMap();
        Hex loc = new(q,r);
        var tile = new Tile(map, loc);

        map.PutTile(tile, loc);

        Assert.Equal(tile, map.TileAt(loc));
    }

    [Fact]
    public void TestMapPutTileOnTile()
    {
        var map = GenMap();
        Hex loc = new(13,13);
        var tile1 = new Tile(map, loc);
        var tile2 = new Tile(map, loc);

        map.PutTile(tile1, loc);

        Assert.Throws<TileAlreadyInitializedException>(
            () => map.PutTile(tile2, loc)
        );
    }

#region "Map Event Handling"
    [Fact]
    public void TestValidTileEventBoundaries()
    {
        var map = GenMap();
        Hex loc = new(14,14);
        var pse = new PlaceStarEvent(1, TurnPhase.PostTurn, loc, 100);

        map.QueueEvent(pse);
    }

    [Fact]
    public void TestInvalidTileEventBoundaries()
    {
        var map = GenMap();
        Hex loc = new(140,140);
        var pse = new PlaceStarEvent(1, TurnPhase.PostTurn, loc, 100);
        
        var ex = Assert.Throws<IndexOutOfRangeException>(() => map.QueueEvent(pse));
    }

    [Fact]
    public void TestInvalidEventTurnNumber()
    {
        var map = GenMap();
        Hex loc = new (14,14);
        var pse = new PlaceStarEvent(1, TurnPhase.PreTurn, loc, 100);

        map.PreTurn();
        map.PostTurn();

        Assert.Equal(2, map.TurnNumber);

        var ex = Assert.Throws<ArgumentException>(() => map.QueueEvent(pse));
        Assert.Equal("Cannot queue an event for a previous turn.", ex.Message);

        var pse2 = new PlaceStarEvent(2, TurnPhase.PreTurn, loc, 100);
        ex = Assert.Throws<ArgumentException>(() => map.QueueEvent(pse2));
        Assert.Equal("Cannot queue a PreTurn event during the same turn.", ex.Message);
    }
#endregion
}
