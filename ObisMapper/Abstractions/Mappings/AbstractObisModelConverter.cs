using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ObisMapper.Mappings;

namespace ObisMapper.Abstractions.Mappings
{
    /// <summary>
    ///     Represents a base class for creating converters that map and transform OBIS models.
    ///     Provides the foundational structure for defining conversion rules for properties within a model,
    ///     enabling custom conversion logic to be implemented in derived classes.
    /// </summary>
    /// <typeparam name="TModel">
    ///     The type of the OBIS model that this converter will handle.
    ///     The model type must implement the <see cref="IObisModel" /> interface.
    /// </typeparam>
    /// <remarks>
    ///     Inherit from this class to create custom converters for specific OBIS models.
    ///     Use the <see cref="RuleFor{TDestination}" /> method to define conversion rules for individual properties in the
    ///     model.
    /// </remarks>
    public abstract class AbstractObisModelConverter<TModel> where TModel : IObisModel
    {
        private readonly Dictionary<PropertyInfo, AbstractRule> _rules = new Dictionary<PropertyInfo, AbstractRule>();

        internal IReadOnlyDictionary<PropertyInfo, AbstractRule> Rules => _rules;

        /// <summary>
        ///     Provides a starting point for defining conversion rules for a specific property of the model.
        ///     This method allows you to specify the target property in the model and then apply various conversion rules or
        ///     validation logic to it.
        /// </summary>
        /// <typeparam name="TDestination">The type of the property in the model that the conversion rule will be applied to.</typeparam>
        /// <param name="propertyExpression">
        ///     An expression that identifies the property of the model for which the conversion rule is being defined.
        /// </param>
        /// <returns>
        ///     A <see cref="ConverterAbstractRule{TDestination}" /> object that allows for further configuration of the conversion
        ///     rules
        ///     and validation for the specified property.
        /// </returns>
        protected ICustomRule<TDestination> RuleFor<TDestination>(
            Expression<Func<TModel, TDestination>> propertyExpression)
        {
            var propertyInfo = (propertyExpression.Body as MemberExpression)?.Member as PropertyInfo;
            // TODO: Add custom exception
            if (propertyInfo == null)
                throw new Exception("IN WORK");

            var rule = new ConverterRule<TDestination>();
            if (!_rules.TryAdd(propertyInfo, rule))
                // TODO: Add custom exception
                throw new Exception("Rule for this property is already exists");
            return rule;
        }
    }
}