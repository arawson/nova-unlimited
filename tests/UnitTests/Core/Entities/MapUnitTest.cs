
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
    [InlineData(1, 1, 1)]
    [InlineData(2, 2, 2)]
    [InlineData(3, 3, 3)]
    public void TestMapCenter(int radius, int q, int r)
    {
        var map = new Map(radius);
        var center = map.Center;
        Assert.Equal(r, center.R);
        Assert.Equal(q, center.Q);
        Assert.Equal(0, center.S);
    }

    [Theory]
    [InlineData(1, 7)]
    [InlineData(2, 19)]
    [InlineData(3, 37)]
    [InlineData(4, 61)]
    public void TestMapArea(int radius, int area)
    {
        var map = new Map(radius);
        Assert.Equal(area, map.Area);
        Assert.Equal(area, map.Count);
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

}
