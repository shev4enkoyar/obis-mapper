using System;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Abstractions.Fluent;

namespace ObisMapper.Fluent.Conversions
{
    internal class ValueConversionHandlerAsync<TDestination> : IConversionHandler<TDestination>
    {
        private readonly Func<object, CancellationToken, Task<TDestination>> _conversionFunc;

        internal ValueConversionHandlerAsync(Func<object, CancellationToken, Task<TDestination>> conversionFunc)
        {
            _conversionFunc = conversionFunc;
        }

        public TDestination Convert(TDestination initial, object value, string? logicalName = null)
        {
            return ConvertAsync(initial, value, logicalName).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<TDestination> ConvertAsync(TDestination initial, object value, string? logicalName = null,
            CancellationToken cancellationToken = default)
        {
            return await _conversionFunc.Invoke(value, cancellationToken).ConfigureAwait(false);
        }
    }
}