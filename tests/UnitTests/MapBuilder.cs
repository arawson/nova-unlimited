
using NovaUnlimited.Core.Entities;

namespace NovaUnlimited.UnitTests;


// TODO: Learn more about test builders:
// TODO: https://ardalis.com/improve-tests-with-the-builder-pattern-for-test-data
// TODO: make the other test builders (Game, etc.)

public class MapBuilder
{
    private Map map;

    public Map Build() => map;

    public MapBuilder(int radius)
    {
        map = new Map(radius);
    }

    public MapBuilder ID(long id) {
        map.Id = id;
        return this;
    }

    // TODO: complete MapBuilder
    // public MapBuilder 
}
