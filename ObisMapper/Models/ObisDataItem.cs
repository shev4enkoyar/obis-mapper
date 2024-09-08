namespace ObisMapper.Models
{
    public class ObisDataItem
    {
        public ObisDataItem(string logicalName, object value)
        {
            LogicalName = logicalName;
            Value = value;
        }
        
        public string LogicalName { get; }

        public object Value { get; }
    }
}