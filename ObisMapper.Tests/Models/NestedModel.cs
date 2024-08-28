namespace ObisMapper.Tests.Models;

public class NestedModel
{
    [LogicalNameMapping("2.1.1.1")] public int NestedIntData { get; set; }

    [LogicalNameMapping("2.1.1.2")] public string NestedStringData { get; set; }

    protected bool Equals(NestedModel other)
    {
        return NestedIntData == other.NestedIntData && NestedStringData == other.NestedStringData;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((NestedModel)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NestedIntData, NestedStringData);
    }

    public static bool operator ==(NestedModel? left, NestedModel? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(NestedModel? left, NestedModel? right)
    {
        return !Equals(left, right);
    }
}