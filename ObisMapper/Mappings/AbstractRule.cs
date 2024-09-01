using System;
using System.Collections.Generic;
using ObisMapper.Models;

namespace ObisMapper.Mappings
{
    internal abstract class AbstractRule
    {
        internal abstract Type DestinationType { get; }

        internal abstract object DefaultValue { get; }

        internal abstract List<LogicalNameModel> LogicalNameModels { get; }

        internal abstract string Tag { get; }
    }
}