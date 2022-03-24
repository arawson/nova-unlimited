
namespace NovaUnlimited.Core.Exceptions;

public class RuleSetNotFoundException : Exception
{
    public RuleSetNotFoundException(int version) : base($"Rule set version {version} not found.") {}
}