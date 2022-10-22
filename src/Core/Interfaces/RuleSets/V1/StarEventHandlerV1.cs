
using NovaUnlimited.Core.Entities;
using NovaUnlimited.Core.Interfaces.RuleSets;
using NovaUnlimited.Core.Interfaces.RuleSets.V1;

namespace NovaUnlimited.Core.RuleSets.V1;

public class StarEventHandlerV1 : IEventHandlerV1
{
    public Game Game { get; init; }

    public IGameRuleSetV1 RuleSet { get; init; }

    public TurnPhase TurnPhase => TurnPhase.PreTurn;

    public void HandleEvent<
}
