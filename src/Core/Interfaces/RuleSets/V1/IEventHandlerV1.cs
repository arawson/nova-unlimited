
using NovaUnlimited.Core.Entities;

namespace NovaUnlimited.Core.Interfaces.RuleSets.V1;

public interface IEventHandlerV1<T>
    : T 
{
    public Game Game { get; init; }
    public IGameRuleSetV1 RuleSet { get; init; }
    public TurnPhase TurnPhase { get; }
}
