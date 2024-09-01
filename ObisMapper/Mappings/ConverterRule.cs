using ObisMapper.Models;

namespace ObisMapper.Mappings
{
    public partial class ConverterRule<TDestination>
    {
        internal ConverterRule()
        {
        }

        internal TDestination DefaultValue { get; private set; } = default!;

        #region Default values

        public ConverterRule<TDestination> AddDefaultValue(TDestination defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        #endregion

        #region Logical names

        public ConverterRule<TDestination> AddLogicalName(LogicalNameModel logicalName)
        {
            return this;
        }

        public ConverterRule<TDestination> AddLogicalName(params LogicalNameModel[] logicalNames)
        {
            return this;
        }

        public ConverterRule<TDestination> AddLogicalName(LogicalNameModelGroup logicalNameGroup)
        {
            return this;
        }

        #endregion
    }
}