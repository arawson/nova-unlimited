
namespace NovaUnlimited.Core.Exceptions;

public class TileAlreadyInitializedException : InvalidOperationException
{
    public TileAlreadyInitializedException(string message) : base(message) {}
}
