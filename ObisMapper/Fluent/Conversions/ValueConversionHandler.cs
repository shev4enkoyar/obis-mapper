using System;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Abstractions.Fluent;

namespace ObisMapper.Fluent.Conversions
{
    internal class ValueConversionHandler<TDestination> : IConversionHandler<TDestination>
    {
        private readonly Func<object, TDestination> _conversionFunc;

        internal ValueConversionHandler(Func<object, TDestination> conversionFunc)
        {
            _conversionFunc = conversionFunc;
        }

        public TDestination Convert(TDestination initial, object value, string? logicalName = null)
        {
            return _conversionFunc(value);
        }

        public Task<TDestination> ConvertAsync(TDestination initial, object value, string? logicalName = null,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Convert(initial, value, logicalName));
        }
    }
}