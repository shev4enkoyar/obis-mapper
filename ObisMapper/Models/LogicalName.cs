using ObisMapper.Constants;

namespace ObisMapper.Models
{
    public struct LogicalName
    {
        public LogicalName(string name, string tag = TagConstant.DefaultTag, string description = "")
        {
            Name = name;
            Description = description;
            Tag = tag;
        }

        public string Name { get; }

        public string Tag { get; }

        public string Description { get; }
    }
}