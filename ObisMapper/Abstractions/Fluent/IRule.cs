using System;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Models;

namespace ObisMapper.Abstractions.Fluent
{
    /// <summary>
    ///     Defines a set of rules for configuring a mapping model with a specific destination type.
    /// </summary>
    /// <typeparam name="TDestination">The type of the destination to map to.</typeparam>
    public interface IModelRule<TDestination>
    {
        IModelRule<TNestedModel> MapNestedModel<TNestedModel>(Action<IModelRule<TNestedModel>> nestedConfigurator) where TNestedModel : class;

        /// <summary>
        ///     Sets the default value for the mapping rule.
        /// </summary>
        /// <param name="defaultValue">The default value to use if no other value is provided.</param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddDefaultValue(TDestination defaultValue);

        /// <summary>
        ///     Adds a tag to the mapping rule for filtering purposes.
        /// </summary>
        /// <param name="tag">The tag to associate with the mapping rule.</param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddTag(string tag);

        /// <summary>
        ///     Marks this mapping rule as the primary rule for the destination type.
        /// </summary>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> IsPrimary();

        #region Logical names

        /// <summary>
        ///     Adds a logical name to the mapping rule.
        /// </summary>
        /// <param name="logicalName">The logical name to associate with the mapping rule.</param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddLogicalName(LogicalName logicalName);

        /// <summary>
        ///     Adds multiple logical names to the mapping rule.
        /// </summary>
        /// <param name="logicalNames">The logical names to associate with the mapping rule.</param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddLogicalName(params LogicalName[] logicalNames);

        /// <summary>
        ///     Adds a group of logical names to the mapping rule.
        /// </summary>
        /// <param name="logicalNameGroup">The group of logical names to associate with the mapping rule.</param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddLogicalName(LogicalNameGroup logicalNameGroup);

        #endregion

        #region Converters

        /// <summary>
        ///     Adds a synchronous conversion handler to the mapping rule.
        /// </summary>
        /// <param name="conversionHandler">The function that converts the source object to the destination type.</param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddConverter(Func<object, TDestination> conversionHandler);

        /// <summary>
        ///     Adds an asynchronous conversion handler to the mapping rule.
        /// </summary>
        /// <param name="conversionHandler">The function that converts the source object to the destination type asynchronously.</param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddConverterAsync(
            Func<object, CancellationToken, Task<TDestination>> conversionHandler);

        /// <summary>
        ///     Adds a synchronous conversion handler that takes the initial value and a source object as parameters.
        /// </summary>
        /// <param name="conversionHandler">
        ///     The function that converts the initial value and the source object to the destination
        ///     type.
        /// </param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddConverter(Func<TDestination, object, TDestination> conversionHandler);

        /// <summary>
        ///     Adds an asynchronous conversion handler that takes the initial value and a source object as parameters.
        /// </summary>
        /// <param name="conversionHandler">
        ///     The function that converts the initial value and the source object to the destination
        ///     type asynchronously.
        /// </param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddConverterAsync(
            Func<TDestination, object, CancellationToken, Task<TDestination>> conversionHandler);

        /// <summary>
        ///     Adds a synchronous conversion handler that takes the initial value, a logical name, and a source object as
        ///     parameters.
        /// </summary>
        /// <param name="conversionHandler">
        ///     The function that converts the initial value, the logical name, and the source object
        ///     to the destination type.
        /// </param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddConverter(Func<TDestination, string, object, TDestination> conversionHandler);

        /// <summary>
        ///     Adds an asynchronous conversion handler that takes the initial value, a logical name, and a source object as
        ///     parameters.
        /// </summary>
        /// <param name="conversionHandler">
        ///     The function that converts the initial value, the logical name, and the source object
        ///     to the destination type asynchronously.
        /// </param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddConverterAsync(
            Func<TDestination, string, object, CancellationToken, Task<TDestination>> conversionHandler);

        #endregion

        #region Validator

        /// <summary>
        ///     Adds a synchronous validator to the mapping rule.
        /// </summary>
        /// <param name="predicate">The function that determines whether the destination value is valid.</param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddValidator(Func<TDestination, bool> predicate);

        /// <summary>
        ///     Adds a synchronous validator to the mapping rule with an error message.
        /// </summary>
        /// <param name="predicate">The function that determines whether the destination value is valid.</param>
        /// <param name="message">The error message to display if the validation fails.</param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddValidator(Func<TDestination, bool> predicate, string message);

        /// <summary>
        ///     Adds an asynchronous validator to the mapping rule.
        /// </summary>
        /// <param name="predicate">The function that asynchronously determines whether the destination value is valid.</param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddValidatorAsync(Func<TDestination, CancellationToken, Task<bool>> predicate);

        /// <summary>
        ///     Adds an asynchronous validator to the mapping rule with an error message.
        /// </summary>
        /// <param name="predicate">The function that asynchronously determines whether the destination value is valid.</param>
        /// <param name="message">The error message to display if the validation fails.</param>
        /// <returns>The current instance of <see cref="IModelRule{TDestination}" />.</returns>
        IModelRule<TDestination> AddValidatorAsync(Func<TDestination, CancellationToken, Task<bool>> predicate,
            string message);

        #endregion
    }
}