namespace ObisMapper.Abstractions
{
    public struct ObisDataModel
    {
        public ObisDataModel(string logicalName, object value)
        {
            LogicalName = logicalName;
            Value = value;
        }

        public string LogicalName { get; }

        public object Value { get; }
    }
}