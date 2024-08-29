using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ObisMapper
{
    /// <summary>
    ///     Represents a custom property mapping for a model of type <typeparamref name="TModel" />,
    ///     allowing for the conversion of property values based on logical names and tags.
    /// </summary>
    public class CustomPropertyMapping<TModel>
    {
        /// <summary>
        ///     Gets the dictionary of mappings between <see cref="LogicalNameMappingAttribute" />
        ///     and their corresponding conversion handlers.
        /// </summary>
        public Dictionary<LogicalNameMappingAttribute, Delegate> Mappings { get; } =
            new Dictionary<LogicalNameMappingAttribute, Delegate>();

        /// <summary>
        ///     Creates a mapping between a property of the model and a conversion handler,
        ///     allowing for custom conversion logic based on the specified tag.
        /// </summary>
        /// <typeparam name="TDestination">The type of the destination property.</typeparam>
        /// <param name="propertyExpression">
        ///     An expression that specifies the property of the model to map.
        /// </param>
        /// <param name="conversionHandler">
        ///     A function that defines how to convert a value for the specified property.
        ///     The first parameter is the current property value, and the second parameter
        ///     is the value to be converted.
        /// </param>
        /// <param name="tag">
        ///     The tag used to distinguish different mappings for the same logical name.
        ///     The default value is <see cref="LogicalNameMappingAttribute.DefaultTag" />.
        /// </param>
        /// <returns>
        ///     The current instance of <see cref="CustomPropertyMapping{TModel}" />, allowing for method chaining.
        /// </returns>
        public CustomPropertyMapping<TModel> CreateMapping<TDestination>(
            Expression<Func<TModel, TDestination>> propertyExpression,
            Func<TDestination, object, TDestination> conversionHandler,
            string tag = LogicalNameMappingAttribute.DefaultTag)
        {
            var propertyInfo = (propertyExpression.Body as MemberExpression)?.Member as PropertyInfo;
            if (propertyInfo == null)
                return this;

            var attributes = propertyInfo
                .GetCustomAttributes<LogicalNameMappingAttribute>(false)
                .ToArray();

            foreach (var attribute in attributes)
                if (tag.Equals(attribute.Tag))
                    Mappings.Add(attribute, conversionHandler);

            return this;
        }
    }
}