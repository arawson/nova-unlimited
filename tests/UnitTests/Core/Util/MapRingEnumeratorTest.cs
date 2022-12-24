
using NovaUnlimited.Core.Entities;
using Xunit;

namespace NovaUnlimited.Core.Util;

public class MapRingEnumeratorUnitTest
{
    [Theory]
    [InlineData(4, 61)]
    // [InlineData(1, 7)]
    public void TestRingEnumeratorLimits(int radius, int area)
    {
        var map = new Map(radius);

        var enumerator = new MapRingEnumerator(map);
        int i = 0;
        for (; i < area; i++)
        {
            if (!enumerator.MoveNext()) break;
        }

        Assert.Equal(area, i);
        Assert.False(enumerator.MoveNext());
    }

    [Theory]
    [InlineData(4)]
    public void TestRingEnumeratorLimitsRange(int radius)
    {
        var map = new Map(radius);
        int i = 0;
        foreach (var _ in map)
        {
            i++;
        }
        Assert.Equal(map.Area, i);
    }
}
