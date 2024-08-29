namespace ObisMapper.Tests.Models;

public class ObisDataItem(string logicalName, object value)
{
    public string LogicalName { get; init; } = logicalName;

    public object Value { get; init; } = value;
}