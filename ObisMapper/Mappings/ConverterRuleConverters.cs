using System;
using ObisMapper.Abstractions.Mappings;
using ObisMapper.Abstractions.Mappings.Converters;

namespace ObisMapper.Mappings
{
    public partial class ConverterRule<TDestination>
    {
        internal IConversionHandler<TDestination>? ConversionHandler;


        public ConverterRule<TDestination> AddConverter(Func<object, TDestination> conversionHandler)
        {
            ConversionHandler = new ValueConversionHandler<TDestination>(conversionHandler);
            return this;
        }

        public ConverterRule<TDestination> AddConverter(Func<TDestination, object, TDestination> conversionHandler)
        {
            ConversionHandler = new InitialValueValueConversionHandler<TDestination>(conversionHandler);
            return this;
        }

        public ConverterRule<TDestination> AddConverter(
            Func<TDestination, string, object, TDestination> conversionHandler)
        {
            ConversionHandler = new InitialValueLogicalNameValueConversionHandler<TDestination>(conversionHandler);
            return this;
        }
    }
}