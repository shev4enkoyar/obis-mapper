using System.Threading;
using System.Threading.Tasks;

namespace ObisMapper.Abstractions.Mappings.Converters
{
    public interface IConversionHandler<TDestination>
    {
        TDestination Convert(TDestination initial, object value, string? logicalName = null);

        Task<TDestination> ConvertAsync(TDestination initial, object value, string? logicalName = null,
            CancellationToken cancellationToken = default);
    }
}