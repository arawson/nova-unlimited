
using NovaUnlimited.Core.Exceptions;
using NovaUnlimited.Core.RuleSets;
using Xunit;

namespace NovaUnlimited.UnitTests.Core.RuleSets;

public class BasicRuleSetFactoryUnitTest
{
    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    public void TestGetRuleSet(int version, bool good)
    {
        RuleSetFactory f = new RuleSetFactory();
        var game = GameUnitTest.GenGame();
        if (good)
        {
            var rs = f.GetRuleSet(version, game);
            Assert.Equal(version, rs.Version);
        }
        else
        {
            Assert.Throws<RuleSetNotFoundException>(
                () => {
                    f.GetRuleSet(version, game);
                }
            );
        }
    }
}
