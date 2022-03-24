
using NovaUnlimited.Core.Entities.Events;

namespace NovaUnlimited.Core.Interfaces;

public interface ITurnable
{
    List<AbstractEvent> ExecutePreTurn(int turnNumber);

    List<AbstractEvent> ExecutePostTurn(int turnNumber);
}
