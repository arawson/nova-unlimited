
using System;
using NovaUnlimited.Core.Entities;
using NovaUnlimited.Core.Entities.Events;
using NovaUnlimited.Core.Util;
using Xunit;

public class GameUnitTest
{
    public const int TestMapRadius = 16;

    public static Game GenGame()
    {
        Game g = new Game(1, new Map(TestMapRadius));

        return g;
    }

    [Fact]
    public void TestGamePreConditions()
    {
        var game = GenGame();

        Assert.Equal(1, game.TurnNumber);
        Assert.Equal(TestMapRadius, game.Map.Radius);
    }

    [Fact]
    public void TestValidTileEventBoundaries()
    {
        var game = GenGame();
        Hex loc = new(14,14);
        var pse = new PlaceStarEvent(1, TurnPhase.PostTurn, loc, 100);

        game.QueueEvent(pse);
    }

    [Fact]
    public void TestInvalidTileEventBoundaries()
    {
        var game = GenGame();
        Hex loc = new(140,140);
        var pse = new PlaceStarEvent(1, TurnPhase.PostTurn, loc, 100);
        
        var ex = Assert.Throws<IndexOutOfRangeException>(() => game.QueueEvent(pse));
    }

    [Fact]
    public void TestInvalidEventTurnNumber()
    {
        var game = GenGame();
        Hex loc = new (14,14);
        var pse = new PlaceStarEvent(1, TurnPhase.PreTurn, loc, 100);

        game.IncrementTurn();

        Assert.Equal(2, game.TurnNumber);

        var ex = Assert.Throws<ArgumentException>(() => game.QueueEvent(pse));
        Assert.Equal("Cannot queue an event for a previous turn.", ex.Message);

        var pse2 = new PlaceStarEvent(2, TurnPhase.PreTurn, loc, 100);
        ex = Assert.Throws<ArgumentException>(() => game.QueueEvent(pse2));
        Assert.Equal("Cannot queue a PreTurn event during the same turn.", ex.Message);
    }
}
