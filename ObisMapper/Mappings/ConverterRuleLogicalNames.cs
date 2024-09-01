using System.Collections.Generic;
using ObisMapper.Models;

namespace ObisMapper.Mappings
{
    internal partial class ConverterRule<TDestination>
    {
        internal override List<LogicalNameModel> LogicalNameModels { get; } = new List<LogicalNameModel>();

        public ICustomRule<TDestination> AddLogicalName(LogicalNameModel logicalName)
        {
            LogicalNameModels.Add(logicalName);
            return this;
        }

        public ICustomRule<TDestination> AddLogicalName(params LogicalNameModel[] logicalNames)
        {
            LogicalNameModels.AddRange(logicalNames);
            return this;
        }

        public ICustomRule<TDestination> AddLogicalName(LogicalNameModelGroup logicalNameGroup)
        {
            return this;
        }
    }
}