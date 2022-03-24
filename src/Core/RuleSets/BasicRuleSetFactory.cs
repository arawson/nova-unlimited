
using NovaUnlimited.Core.Exceptions;
using NovaUnlimited.Core.Entities;
using NovaUnlimited.Core.RuleSets.V1;
using NovaUnlimited.Core.Interfaces;
using NovaUnlimited.Core.Interfaces.RuleSets;

namespace NovaUnlimited.Core.RuleSets;

public class RuleSetFactory : IGameRuleSetFactory
{
    public IGameRuleSet GetRuleSet(int version, Game game)
    {
        switch (version)
        {
            case 1:
                return new NovaUnlimitedV1(game);
            
            default:
                throw new RuleSetNotFoundException(version);
        }
    }
}
