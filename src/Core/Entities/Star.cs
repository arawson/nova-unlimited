
using Ardalis.GuardClauses;
using NovaUnlimited.Core.Entities.Events;
using NovaUnlimited.Core.Interfaces;

namespace NovaUnlimited.Core.Entities;

public class Star
{
    public int InitialLifetime { get; private init; }
    public int RemainingLifetime { get; private set; }

    public Star(int initialLifetime)
    {
        Guard.Against.NegativeOrZero(initialLifetime, nameof(initialLifetime));

        InitialLifetime = initialLifetime;
        RemainingLifetime = initialLifetime;
    }
}
