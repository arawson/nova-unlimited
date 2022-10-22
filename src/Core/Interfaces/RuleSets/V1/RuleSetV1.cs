
using Ardalis.GuardClauses;
using NovaUnlimited.Core.Entities;
using NovaUnlimited.Core.Entities.Events;
using NovaUnlimited.Core.Interfaces.RuleSets;

namespace NovaUnlimited.Core.RuleSets.V1;

public class RuleSetV1 : IGameRuleSetV1
{
    public int Version => 1;

    public int TurnNumber => Game.TurnNumber;

    public Game Game { get; private init; }

    public RuleSetV1(
        Game game
    )
    {
        Guard.Against.Null(game, nameof(game));

        this.Game = game;
    }

    public List<AbstractEvent> ExecutePostTurn(int turnNumber)
    {
        throw new NotImplementedException();

        Game.IncrementTurn();
    }

    public List<AbstractEvent> ExecutePreTurn(int turnNumber)
    {
        throw new NotImplementedException();
    }

    public void QueueEvent(AbstractEvent e)
    {
        Game.QueueEvent(e);
    }
}
