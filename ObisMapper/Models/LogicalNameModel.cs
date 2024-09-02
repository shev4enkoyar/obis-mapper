using ObisMapper.Constants;

namespace ObisMapper.Models
{
    public struct LogicalNameModel
    {
        public LogicalNameModel(string logicalName, string tag = TagConstant.DefaultTag, string description = "")
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