
namespace NovaUnlimited.Core.Interfaces.Events;

public interface ITurnEventListener
{
    void Handle(ITurnEvent e);
}
