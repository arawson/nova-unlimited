
using Ardalis.GuardClauses;
using NovaUnlimited.Core.Entities;
using NovaUnlimited.Core.Entities.Events;
using NovaUnlimited.Core.Interfaces.Events;
using NovaUnlimited.Core.Interfaces.RuleSets;
using NovaUnlimited.Core.Interfaces.RuleSets.V1;

namespace NovaUnlimited.Core.RuleSets.V1;

public class PlaceStarEventHandlerV1 : IPlaceStarEventListener, IEventHandlerV1<PlaceStarEvent>
{
    public Game Game { get; init; }
    public IGameRuleSetV1 RuleSet { get; init; }
    public TurnPhase TurnPhase => TurnPhase.PreTurn;

    public PlaceStarEventHandlerV1(Game game, IGameRuleSetV1 ruleSet)
    {
        Game = game;
        RuleSet = ruleSet;
    }

    public void Handle(PlaceStarEvent e)
    {
        var tile = Game.Map.TileAt(e.Location);
        Guard.Against.Null(tile, nameof(tile));
    
        tile.Contents = TileType.Star;
        tile.Star = new Star(Random.Shared.Next() % 30);
    }
}
