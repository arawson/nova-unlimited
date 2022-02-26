
using NovaUnlimited.Core.Entities.Events;

namespace NovaUnlimited.Core.Interfaces;

public interface ITurnable
{
    List<AbstractEvent> PreTurn(int turnNumber);

    List<AbstractEvent> PostTurn(int turnNumber);
}
