namespace ObisMapper.Models
{
    public struct LogicalNameModelGroup
    {
        public LogicalNameModel[] LogicalNames { get; }

        public LogicalNameModelGroup(LogicalNameModel[] logicalNames)
        {
            LogicalNames = logicalNames;
        }
    }
}