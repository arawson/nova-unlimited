
using NovaUnlimited.Core.Entities;

namespace NovaUnlimited.Core.Interfaces.RuleSets;

public interface IGameRuleSetFactory
{
    IGameRuleSet GetRuleSet(int version, Game game);
}
