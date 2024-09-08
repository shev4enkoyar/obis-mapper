using System;
using ObisMapper.Constants;

namespace ObisMapper.Models
{
    public struct LogicalNameGroup : IEquatable<LogicalNameGroup>
    {
        public LogicalNameGroup(params LogicalName[] logicalNames)
        {
            LogicalNames = logicalNames;
        }

        public LogicalName[] LogicalNames { get; }

        public bool Equals(LogicalNameGroup other)
        {
            return LogicalNames.Equals(other.LogicalNames);
        }

        public override bool Equals(object? obj)
        {
            return obj is LogicalNameGroup other && Equals(other);
        }

        public override int GetHashCode()
        {
            // TODO: Custom dictionary with two GetHashCode (GetHashCode(LogicalName) or GetHashCode(LogicalName + Tag))
            // In: Index by name / Index by name + tag
            return HashCode.Combine(LogicalNames);
        }

        public static bool operator ==(LogicalNameGroup left, LogicalNameGroup right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LogicalNameGroup left, LogicalNameGroup right)
        {
            return !left.Equals(right);
        }
    }
}