
namespace NovaUnlimited.Kernel.Serialization;

public abstract record class ArrayEntry<T, O>
{
    public long OwnerId { get; set; }
    public virtual O Owner { get; set; }
    public int IndexX { get; set; }
    public int IndexY { get; set; }
    public T Value { get; set; }
}
