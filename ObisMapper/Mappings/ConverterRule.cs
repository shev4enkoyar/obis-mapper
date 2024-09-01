using System;
using ObisMapper.Abstractions;

namespace ObisMapper.Mappings
{
    internal partial class ConverterRule<TDestination> : AbstractRule, ICustomRule<TDestination>
    {
        private string _tag = "";
        internal IConversionHandler<TDestination>? ConversionHandler { get; private set; }

        internal IValidationHandler<TDestination>? ValidationHandler { get; private set; }


        internal override Type DestinationType => typeof(TDestination);

        internal override string Tag => _tag;


        #region Default values

        public ICustomRule<TDestination> AddDefaultValue(TDestination defaultValue)
        {
            _defaultValue = defaultValue;
            return this;
        }

        public ICustomRule<TDestination> AddTag(string tag)
        {
            _tag = tag;
            return this;
        }

        private TDestination _defaultValue = default!;
        internal override object DefaultValue => _defaultValue;

        #endregion
    }
}