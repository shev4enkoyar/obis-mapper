using System;
using System.Threading;
using System.Threading.Tasks;

namespace ObisMapper.Abstractions.Mappings.Converters
{
    internal class InitialValueLogicalNameValueConversionHandlerAsync<TDestination> : IConversionHandler<TDestination>
    {
        private readonly Func<TDestination, string, object, CancellationToken, Task<TDestination>> _conversionFunc;

        internal InitialValueLogicalNameValueConversionHandlerAsync(
            Func<TDestination, string, object, CancellationToken, Task<TDestination>> conversionFunc)
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
            if (logicalName == null) throw new ArgumentNullException(nameof(logicalName));
            return await _conversionFunc.Invoke(initial, logicalName, value, cancellationToken).ConfigureAwait(false);
        }
    }
}