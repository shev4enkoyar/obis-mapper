namespace ObisMapper.Models
{
    public struct LogicalNameGroup
    {
        public LogicalName[] LogicalNames { get; }

        public LogicalNameGroup(LogicalName[] logicalNames)
        {
            LogicalNames = logicalNames;
        }
    }
}