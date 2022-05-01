
using NovaUnlimited.Core.Util;
using Xunit;

namespace NovaUnlimited.UnitTests.Core.Util;

public class HexUnitTest
{
    [Theory]
    [InlineData(1,2,-3)]
    [InlineData(1,-2,1)]
    [InlineData(0,0,0)]
    public void TestHexSCoord(int q, int r, int s)
    {
        Hex h = new(q,r);
        Assert.Equal(s, h.S);
    }

    [Theory]
    [InlineData(0,0,5,0,5)]
    [InlineData(4,-4,4,-1,3)]
    public void TestHexDistance(int q1, int r1, int q2, int r2, int d)
    {
        Hex a = new(q1, r1);
        Hex b = new(q2, r2);
        Assert.Equal(d, a.Distance(b));
    }
}
