using System.Threading;
using System.Threading.Tasks;

namespace ObisMapper.Abstractions.Fluent
{
    /// <summary>
    ///     Defines an interface for validating values of a specific type.
    ///     Implementations of this interface can be used to perform both synchronous and asynchronous validation.
    /// </summary>
    /// <typeparam name="TDestination">The type of the value to be validated.</typeparam>
    public interface IValidationHandler<in TDestination>
    {
        /// <summary>
        ///     Validates the specified value synchronously.
        /// </summary>
        /// <param name="value">The value of type <typeparamref name="TDestination" /> to validate.</param>
        /// <returns><c>true</c> if the value is valid; otherwise, <c>false</c>.</returns>
        bool Validate(TDestination value);

        /// <summary>
        ///     Validates the specified value asynchronously.
        /// </summary>
        /// <param name="value">The value of type <typeparamref name="TDestination" /> to validate.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        ///     A task that represents the asynchronous validation operation.
        ///     The task result contains <c>true</c> if the value is valid; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> ValidateAsync(TDestination value, CancellationToken cancellationToken = default);
    }
}