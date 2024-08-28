namespace ObisMapper.Tests.Models;

public class SimpleModel
{
    [LogicalNameMapping("1.1.1.1")] public int FirstNumericData { get; set; }

    [LogicalNameMapping("1.1.1.2")] public int SecondNumericData { get; set; }

    [LogicalNameMapping("1.1.2.1")] public string FirstStringData { get; set; }

    [LogicalNameMapping("1.1.2.2")] public string SecondStringData { get; set; }

    [LogicalNameMapping("1.1.3.1")] public double FirstDoubleData { get; set; }

    [LogicalNameMapping("1.1.3.2")] public double SecondDoubleData { get; set; }

    protected bool Equals(SimpleModel other)
    {
        return FirstNumericData == other.FirstNumericData && SecondNumericData == other.SecondNumericData &&
               FirstStringData == other.FirstStringData && SecondStringData == other.SecondStringData &&
               FirstDoubleData.Equals(other.FirstDoubleData) && SecondDoubleData.Equals(other.SecondDoubleData);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SimpleModel)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstNumericData, SecondNumericData, FirstStringData, SecondStringData,
            FirstDoubleData, SecondDoubleData);
    }

    public static bool operator ==(SimpleModel? left, SimpleModel? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(SimpleModel? left, SimpleModel? right)
    {
        return !Equals(left, right);
    }
}