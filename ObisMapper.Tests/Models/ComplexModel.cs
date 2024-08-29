namespace ObisMapper.Tests.Models;

public class ComplexModel
{
    [LogicalNameMapping("1.1.1.1")] public int NumericValue { get; set; }

    [LogicalNameMapping("1.1.1.2")] public List<int> EnumeratedValue { get; set; } = [];

    protected bool Equals(ComplexModel other)
    {
        return NumericValue == other.NumericValue && EnumeratedValue.SequenceEqual(other.EnumeratedValue);
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
        return HashCode.Combine(NumericValue, EnumeratedValue);
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