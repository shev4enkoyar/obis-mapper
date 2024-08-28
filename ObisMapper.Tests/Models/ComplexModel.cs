namespace ObisMapper.Tests.Models;

public class ComplexModel
{
    [LogicalNameMapping("1.1.1.1")] public int? SimpleData { get; set; }

    [NestedModel] public NestedModel NestedData { get; set; }

    protected bool Equals(ComplexModel other)
    {
        return SimpleData == other.SimpleData && NestedData.Equals(other.NestedData);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ComplexModel)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SimpleData, NestedData);
    }

    public static bool operator ==(ComplexModel? left, ComplexModel? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ComplexModel? left, ComplexModel? right)
    {
        return !Equals(left, right);
    }
}