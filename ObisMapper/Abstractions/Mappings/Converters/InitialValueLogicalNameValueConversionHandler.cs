using System;
using System.Threading;
using System.Threading.Tasks;

namespace ObisMapper.Abstractions.Mappings.Converters
{
    internal class InitialValueLogicalNameValueConversionHandler<TDestination> : IConversionHandler<TDestination>
    {
        private readonly Func<TDestination, string, object, TDestination> _conversionFunc;

        internal InitialValueLogicalNameValueConversionHandler(
            Func<TDestination, string, object, TDestination> conversionFunc)
        {
            _conversionFunc = conversionFunc;
        }

        public TDestination Convert(TDestination initial, object value, string? logicalName = null)
        {
            if (logicalName == null) throw new ArgumentNullException(nameof(logicalName));
            return _conversionFunc.Invoke(initial, logicalName, value);
        }

        public Task<TDestination> ConvertAsync(TDestination initial, object value, string? logicalName = null,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Convert(initial, value, logicalName));
        }
    }
}