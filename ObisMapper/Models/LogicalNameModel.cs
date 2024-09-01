namespace ObisMapper.Models
{
    public struct LogicalNameModel
    {
        public const string DefaultTag = "";

        public LogicalNameModel(string logicalName, string tag = DefaultTag, string description = "")
        {
            LogicalName = logicalName;
            Description = description;
            Tag = tag;
        }

        public string LogicalName { get; }

        public string Tag { get; }

        public string Description { get; }
    }
}