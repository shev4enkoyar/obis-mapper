using System;
using System.Collections.Generic;
using ObisMapper.Models;

namespace ObisMapper.Fluent
{
    internal abstract class BaseModelRule
    {
        internal abstract Type DestinationType { get; }

        internal abstract object? DefaultValue { get; }

        internal abstract List<LogicalNameModel> LogicalNameModels { get; }

        internal abstract string Tag { get; }

        internal abstract bool IsPrimary { get; }
    }
}