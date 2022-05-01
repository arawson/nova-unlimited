
namespace NovaUnlimited.Kernel.Interfaces;

/// <summary>
/// Apply this marker interface only to aggregate root entities.
/// Repositories will only work aggregate roots, not their children.
/// </summary>
public interface IAggregateRoot { }
