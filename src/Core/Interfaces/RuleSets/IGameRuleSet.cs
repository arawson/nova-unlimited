
using NovaUnlimited.Core.Interfaces.Events;

namespace NovaUnlimited.Core.Interfaces.RuleSets;

public interface IGameRuleSet : ITurnable, IEventContainer
{
    int Version { get; }
}
