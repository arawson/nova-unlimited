
using NovaUnlimited.Core.Util;

namespace NovaUnlimited.Core.Interfaces.Events;

public interface ITileEvent
{
    Hex Location { get; }
}
