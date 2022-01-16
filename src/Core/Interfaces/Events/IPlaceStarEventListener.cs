
using NovaUnlimited.Core.Entities.Events;

namespace NovaUnlimited.Core.Interfaces.Events;

public interface IPlaceStarEventListener
{
    void Handle(PlaceStarEvent e);
}
