using System.Threading;
using System.Threading.Tasks;

namespace ObisMapper.Abstractions.Fluent
{
    /// <summary>
    ///     Defines methods for converting values to a specified destination type.
    /// </summary>
    /// <typeparam name="TDestination">The type of the destination to convert to.</typeparam>
    public interface IConversionHandler<TDestination>
    {
        /// <summary>
        ///     Synchronously converts a value to the specified destination type.
        /// </summary>
        /// <param name="initial">The initial value of the destination type.</param>
        /// <param name="value">The source value to convert.</param>
        /// <param name="logicalName">An optional logical name that may influence the conversion.</param>
        /// <returns>The converted value of type <typeparamref name="TDestination" />.</returns>
        TDestination Convert(TDestination initial, object value, string? logicalName = null);

        /// <summary>
        ///     Asynchronously converts a value to the specified destination type.
        /// </summary>
        /// <param name="initial">The initial value of the destination type.</param>
        /// <param name="value">The source value to convert.</param>
        /// <param name="logicalName">An optional logical name that may influence the conversion.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>
        ///     A task representing the asynchronous conversion operation, containing the converted value of type
        ///     <typeparamref name="TDestination" />.
        /// </returns>
        Task<TDestination> ConvertAsync(TDestination initial, object value, string? logicalName = null,
            CancellationToken cancellationToken = default);
    }
}