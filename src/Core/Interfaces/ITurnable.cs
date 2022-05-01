
using NovaUnlimited.Core.Entities.Events;

namespace NovaUnlimited.Core.Interfaces;

public interface ITurnable
{
    List<BaseEvent> ExecutePreTurn(int turnNumber);

    List<BaseEvent> ExecutePostTurn(int turnNumber);
}
