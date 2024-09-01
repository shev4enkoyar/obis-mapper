using System;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Abstractions.Fluent;

namespace ObisMapper.Fluent.Conversions
{
    internal class InitialValueValueConversionHandlerAsync<TDestination> : IConversionHandler<TDestination>
    {
        private readonly Func<TDestination, object, CancellationToken, Task<TDestination>> _conversionFunc;

        internal InitialValueValueConversionHandlerAsync(
            Func<TDestination, object, CancellationToken, Task<TDestination>> conversionFunc)
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
            return await _conversionFunc.Invoke(initial, value, cancellationToken).ConfigureAwait(false);
        }
    }
}