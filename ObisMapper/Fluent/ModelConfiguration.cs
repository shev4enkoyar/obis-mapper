using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ObisMapper.Abstractions.Fluent;
using ObisMapper.Exceptions;

namespace ObisMapper.Fluent
{
    /// <summary>
    ///     Provides a base class for configuring model rules in an OBIS model.
    ///     This class allows you to define how properties of the model should be converted or validated
    ///     during the mapping process.
    /// </summary>
    /// <typeparam name="TModel">The type of the model that this configuration applies to.</typeparam>
    public abstract class ModelConfiguration<TModel> where TModel : IObisModel
    {
        private readonly Dictionary<PropertyInfo, BaseModelRule> _rules = new Dictionary<PropertyInfo, BaseModelRule>();
        internal IReadOnlyDictionary<PropertyInfo, BaseModelRule> Rules => _rules;

        /// <summary>
        ///     Defines a conversion rule for a specific property of the model.
        ///     This method allows you to specify how a property should be handled during the mapping process.
        /// </summary>
        /// <typeparam name="TDestination">The type of the destination value for the property.</typeparam>
        /// <param name="propertyExpression">An expression that identifies the property for which the rule is being defined.</param>
        /// <returns>An <see cref="IModelRule{TDestination}" /> instance that allows further configuration of the rule.</returns>
        /// <exception cref="ExpressionException">
        ///     Thrown if the property information cannot be retrieved from the provided
        ///     expression.
        /// </exception>
        /// <exception cref="ArgumentException">Thrown if a rule for the specified property has already been defined.</exception>
        protected IModelRule<TDestination> RuleFor<TDestination>(
            Expression<Func<TModel, TDestination>> propertyExpression)
        {
            var propertyInfo = (propertyExpression.Body as MemberExpression)?.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ExpressionException("Failed to get model's property information.");

            var rule = new ModelRule<TDestination>();
            if (!_rules.TryAdd(propertyInfo, rule))
                throw new ArgumentException("Rule for this property is already exists.");
            return rule;
        }
    }
}