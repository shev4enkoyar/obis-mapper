using ObisMapper.Abstractions.Fluent;
using ObisMapper.Attributes;

namespace ObisMapper.Tests.Models;

public class ModelWithNestedModel : IObisModel
{
    [LogicalNameMapping("1.1.1.1")] public int? SimpleData { get; set; }

    [NestedModel] public NestedModel NestedData { get; set; }

    private bool Equals(ModelWithNestedModel other)
    {
        return SimpleData == other.SimpleData && NestedData.Equals(other.NestedData);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ModelWithNestedModel)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SimpleData, NestedData);
    }

    public static bool operator ==(ModelWithNestedModel? left, ModelWithNestedModel? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ModelWithNestedModel? left, ModelWithNestedModel? right)
    {
        return !Equals(left, right);
    }
}