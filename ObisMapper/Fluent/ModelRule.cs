using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Abstractions.Fluent;
using ObisMapper.Fluent.Conversions;
using ObisMapper.Fluent.Validators;
using ObisMapper.Models;

namespace ObisMapper.Fluent
{
    internal class ModelRule<TDestination> : BaseModelRule, IModelRule<TDestination>
    {
        private string _tag = "";
        internal IConversionHandler<TDestination>? ConversionHandler { get; private set; }

        internal List<IValidationHandler<TDestination>> ValidationHandlers { get; } =
            new List<IValidationHandler<TDestination>>();


        internal override Type DestinationType => typeof(TDestination);

        internal override string Tag => _tag;

        internal override bool IsPrimary => _isPrimary;


        #region Default values

        public IModelRule<TDestination> AddDefaultValue(TDestination defaultValue)
        {
            _defaultValue = defaultValue;
            return this;
        }

        public IModelRule<TDestination> AddTag(string tag)
        {
            _tag = tag;
            return this;
        }

        IModelRule<TDestination> IModelRule<TDestination>.IsPrimary()
        {
            _isPrimary = true;
            return this;
        }

        private TDestination _defaultValue = default!;
        private bool _isPrimary;
        internal override object? DefaultValue => _defaultValue;

        #endregion

        #region Conversions

        public IModelRule<TDestination> AddConverter(Func<object, TDestination> conversionHandler)
        {
            ConversionHandler = new ValueConversionHandler<TDestination>(conversionHandler);
            return this;
        }

        public IModelRule<TDestination> AddConverterAsync(
            Func<object, CancellationToken, Task<TDestination>> conversionHandler)
        {
            ConversionHandler = new ValueConversionHandlerAsync<TDestination>(conversionHandler);
            return this;
        }

        public IModelRule<TDestination> AddConverter(Func<TDestination, object, TDestination> conversionHandler)
        {
            ConversionHandler = new InitialValueValueConversionHandler<TDestination>(conversionHandler);
            return this;
        }

        public IModelRule<TDestination> AddConverterAsync(
            Func<TDestination, object, CancellationToken, Task<TDestination>> conversionHandler)
        {
            ConversionHandler = new InitialValueValueConversionHandlerAsync<TDestination>(conversionHandler);
            return this;
        }

        public IModelRule<TDestination> AddConverter(
            Func<TDestination, string, object, TDestination> conversionHandler)
        {
            ConversionHandler = new InitialValueLogicalNameValueConversionHandler<TDestination>(conversionHandler);
            return this;
        }

        public IModelRule<TDestination> AddConverterAsync(
            Func<TDestination, string, object, CancellationToken, Task<TDestination>> conversionHandler)
        {
            ConversionHandler = new InitialValueLogicalNameValueConversionHandlerAsync<TDestination>(conversionHandler);
            return this;
        }

        #endregion

        #region Logical names

        internal override List<LogicalNameModel> LogicalNameModels { get; } = new List<LogicalNameModel>();

        public IModelRule<TDestination> AddLogicalName(LogicalNameModel logicalName)
        {
            LogicalNameModels.Add(logicalName);
            return this;
        }

        public IModelRule<TDestination> AddLogicalName(params LogicalNameModel[] logicalNames)
        {
            LogicalNameModels.AddRange(logicalNames);
            return this;
        }

        public IModelRule<TDestination> AddLogicalName(LogicalNameModelGroup logicalNameGroup)
        {
            return this;
        }

        #endregion

        #region Validations

        /// <summary>
        ///     Adds a synchronous validation rule to the converter. The provided predicate function will be used to validate the
        ///     converted value after conversion.
        /// </summary>
        /// <param name="predicate">
        ///     A function that defines the validation logic. It takes the converted value as input and returns
        ///     a boolean indicating whether the value is valid.
        /// </param>
        /// <returns>The current instance of <see cref="ModelRule{TDestination}" />, allowing for method chaining.</returns>
        public IModelRule<TDestination> AddValidator(Func<TDestination, bool> predicate)
        {
            ValidationHandlers.Add(new ConverterRuleValidatorHandler<TDestination>(predicate, null));
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
        /// <returns>The current instance of <see cref="ModelRule{TDestination}" />, allowing for method chaining.</returns>
        public IModelRule<TDestination> AddValidator(Func<TDestination, bool> predicate, string message)
        {
            ValidationHandlers.Add(new ConverterRuleValidatorHandler<TDestination>(predicate, message));
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
        /// <returns>The current instance of <see cref="ModelRule{TDestination}" />, allowing for method chaining.</returns>
        public IModelRule<TDestination> AddValidatorAsync(
            Func<TDestination, CancellationToken, Task<bool>> predicate)
        {
            ValidationHandlers.Add(new ConverterRuleValidatorHandlerAsync<TDestination>(predicate, null));
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
        /// <returns>The current instance of <see cref="ModelRule{TDestination}" />, allowing for method chaining.</returns>
        public IModelRule<TDestination> AddValidatorAsync(
            Func<TDestination, CancellationToken, Task<bool>> predicate, string message)
        {
            ValidationHandlers.Add(new ConverterRuleValidatorHandlerAsync<TDestination>(predicate, message));
            return this;
        }

        #endregion
    }
}