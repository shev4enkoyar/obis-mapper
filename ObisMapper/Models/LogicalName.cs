using System;
using ObisMapper.Constants;

namespace ObisMapper.Models
{
    public struct LogicalName : IEquatable<LogicalName>
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

        public bool Equals(LogicalName other)
        {
            return Name.Equals(other.Name) && Tag.Equals(other.Tag);
        }

        public override bool Equals(object? obj)
        {
            return obj is LogicalName other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Tag);
        }

        public static bool operator ==(LogicalName left, LogicalName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LogicalName left, LogicalName right)
        {
            return !left.Equals(right);
        }
    }
}