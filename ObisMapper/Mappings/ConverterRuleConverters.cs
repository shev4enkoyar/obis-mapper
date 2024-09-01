using System;
using System.Threading;
using System.Threading.Tasks;
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
        
        public ICustomRule<TDestination> AddConverterAsync(Func<object, CancellationToken, Task<TDestination>> conversionHandler)
        {
            ConversionHandler = new ValueConversionHandlerAsync<TDestination>(conversionHandler);
            return this;
        }
        
        public ICustomRule<TDestination> AddConverter(Func<TDestination, object, TDestination> conversionHandler)
        {
            ConversionHandler = new InitialValueValueConversionHandler<TDestination>(conversionHandler);
            return this;
        }

        public ICustomRule<TDestination> AddConverterAsync(Func<TDestination, object, CancellationToken, Task<TDestination>> conversionHandler)
        {
            ConversionHandler = new InitialValueValueConversionHandlerAsync<TDestination>(conversionHandler);
            return this;
        }

        public ICustomRule<TDestination> AddConverter(
            Func<TDestination, string, object, TDestination> conversionHandler)
        {
            ConversionHandler = new InitialValueLogicalNameValueConversionHandler<TDestination>(conversionHandler);
            return this;
        }
        
        public ICustomRule<TDestination> AddConverterAsync(
            Func<TDestination, string, object, CancellationToken, Task<TDestination>> conversionHandler)
        {
            ConversionHandler = new InitialValueLogicalNameValueConversionHandlerAsync<TDestination>(conversionHandler);
            return this;
        }
    }
}