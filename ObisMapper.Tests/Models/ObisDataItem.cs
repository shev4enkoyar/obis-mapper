namespace ObisMapper.Tests.Models;

public class ObisDataItem(string logicalName, object value)
{
    public string LogicalName { get; } = logicalName;

    public object Value { get; } = value;
}