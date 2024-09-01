using System;
using System.Threading;
using System.Threading.Tasks;

namespace ObisMapper.Abstractions.Mappings.Converters
{
    internal class InitialValueValueConversionHandler<TDestination> : IConversionHandler<TDestination>
    {
        private readonly Func<TDestination, object, TDestination> _conversionFunc;

        internal InitialValueValueConversionHandler(Func<TDestination, object, TDestination> conversionFunc)
        {
            _conversionFunc = conversionFunc;
        }

        public TDestination Convert(TDestination initial, object value, string? logicalName = null)
        {
            return _conversionFunc.Invoke(initial, value);
        }

        public Task<TDestination> ConvertAsync(TDestination initial, object value, string? logicalName = null,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Convert(initial, value, logicalName));
        }
    }
}