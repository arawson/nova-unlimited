
using System.Linq;
using System.Threading.Tasks;
using NovaUnlimited.Core.Entities;
using Xunit;

namespace NovaUnlimited.IntegrationTests.Data;

public class EfRepositoryAdd : BaseEfRepoTestFixture
{
    [Fact]
    public async Task AddsGame()
    {
        var map = new Map(15);
        var game = new Game(1, map);
        var repository = GetRepository();

        await repository.AddAsync(game);

        var newgame = (await repository.ListAsync()).FirstOrDefault();

        Assert.NotNull(newgame);
        Assert.Equal(game.Map.Radius, newgame?.Map.Radius);
        Assert.Equal(game.TurnNumber, newgame?.TurnNumber);
        Assert.Equal(game.Map.Id, newgame!.Map.Id);
        Assert.Equal(game.Map.Tiles.LongLength, newgame.Map.Tiles.LongLength);
    }
}
