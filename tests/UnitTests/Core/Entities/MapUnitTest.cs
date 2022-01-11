
namespace NovaUnlimited.Core.Entities;

using System;
using NovaUnlimited.Core.Util;
using Xunit;

public class MapUnitTest
{
    private const int _testRadius = 13;

    private static Map GenMap()
    {
        return new Map(_testRadius);
    }

    [Fact]
    public void TestMapDimensions()
    {
        // Given
        var map = GenMap();
        // When
    
        // Then
        Assert.Equal(27, map.Diameter);
        Assert.Equal(_testRadius, map.Radius);
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

        Assert.Throws<ArgumentException>(() =>
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
}
