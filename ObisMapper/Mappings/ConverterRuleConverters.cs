using System;
using ObisMapper.Abstractions.Mappings.Converters;

namespace ObisMapper.Mappings
{
    internal partial class ConverterRule<TDestination>
    {
        public ICustomRule<TDestination> AddConverter(Func<object, TDestination> conversionHandler)
        {
            ConversionHandler = new ValueConversionHandler<TDestination>(conversionHandler);
            return this;
        }

        public ICustomRule<TDestination> AddConverter(Func<TDestination, object, TDestination> conversionHandler)
        {
            ConversionHandler = new InitialValueValueConversionHandler<TDestination>(conversionHandler);
            return this;
        }

        public ICustomRule<TDestination> AddConverter(
            Func<TDestination, string, object, TDestination> conversionHandler)
        {
            ConversionHandler = new InitialValueLogicalNameValueConversionHandler<TDestination>(conversionHandler);
            return this;
        }
    }
}