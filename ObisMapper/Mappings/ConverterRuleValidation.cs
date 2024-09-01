using System;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Abstractions.Mappings.Validators;

namespace ObisMapper.Mappings
{
    public partial class ConverterRule<TDestination>
    {
        internal IValidationHandler<TDestination> ValidationHandler;

        /// <summary>
        ///     Adds a synchronous validation rule to the converter. The provided predicate function will be used to validate the
        ///     converted value after conversion.
        /// </summary>
        /// <param name="predicate">
        ///     A function that defines the validation logic. It takes the converted value as input and returns
        ///     a boolean indicating whether the value is valid.
        /// </param>
        /// <returns>The current instance of <see cref="ConverterRule{TDestination}" />, allowing for method chaining.</returns>
        public ConverterRule<TDestination> AddValidator(Func<TDestination, bool> predicate)
        {
            ValidationHandler = new ConverterRuleValidatorHandler<TDestination>(predicate, null);
            return this;
        }

        /// <summary>
        ///     Adds a synchronous validation rule to the converter with a custom error message. The provided predicate function
        ///     will be used to validate the converted value after conversion.
        /// </summary>
        /// <param name="predicate">
        ///     A function that defines the validation logic. It takes the converted value as input and returns
        ///     a boolean indicating whether the value is valid.
        /// </param>
        /// <param name="message">A custom error message to be used if validation fails.</param>
        /// <returns>The current instance of <see cref="ConverterRule{TDestination}" />, allowing for method chaining.</returns>
        public ConverterRule<TDestination> AddValidator(Func<TDestination, bool> predicate, string message)
        {
            ValidationHandler = new ConverterRuleValidatorHandler<TDestination>(predicate, message);
            return this;
        }

        /// <summary>
        ///     Adds an asynchronous validation rule to the converter. The provided asynchronous predicate function will be used to
        ///     validate the
        ///     converted value after conversion.
        /// </summary>
        /// <param name="predicate">
        ///     An asynchronous function that defines the validation logic. It takes the converted value and a
        ///     <see cref="CancellationToken" /> as input and returns a Task&lt;bool&gt; indicating whether the value is valid.
        /// </param>
        /// <returns>The current instance of <see cref="ConverterRule{TDestination}" />, allowing for method chaining.</returns>
        public ConverterRule<TDestination> AddValidator(
            Func<TDestination, CancellationToken, Task<bool>> predicate)
        {
            ValidationHandler = new ConverterRuleValidatorHandlerAsync<TDestination>(predicate, null);
            return this;
        }

        /// <summary>
        ///     Adds an asynchronous validation rule to the converter with a custom error message. The provided asynchronous
        ///     predicate function will be used to validate the converted value after conversion.
        /// </summary>
        /// <param name="predicate">
        ///     An asynchronous function that defines the validation logic. It takes the converted value and a
        ///     <see cref="CancellationToken" /> as input and returns a Task&lt;bool&gt; indicating whether the value is valid.
        /// </param>
        /// <param name="message">A custom error message to be used if validation fails.</param>
        /// <returns>The current instance of <see cref="ConverterRule{TDestination}" />, allowing for method chaining.</returns>
        public ConverterRule<TDestination> AddValidator(
            Func<TDestination, CancellationToken, Task<bool>> predicate, string message)
        {
            ValidationHandler = new ConverterRuleValidatorHandlerAsync<TDestination>(predicate, message);
            return this;
        }
    }
}