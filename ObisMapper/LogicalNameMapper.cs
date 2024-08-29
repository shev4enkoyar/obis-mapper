using System;
using System.Linq;
using System.Reflection;
using ObisMapper.Attributes;

namespace ObisMapper
{
    /// <summary>
    ///     Provides functionality to map values to model properties based on logical names,
    ///     with support for nested models and default values.
    /// </summary>
    public static class LogicalNameMapper
    {
        /// <summary>
        ///     Fills the properties of the specified model with the provided value,
        ///     based on the logical name and tag. Recursively processes nested models
        ///     marked with <see cref="NestedModelAttribute" />.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="model">The model to fill.</param>
        /// <param name="logicalName">The logical name to match for property filling.</param>
        /// <param name="value">The value to set for the matching properties.</param>
        /// <param name="tag">
        ///     The tag to match for property filling (default is
        ///     <see cref="LogicalNameMappingAttribute.DefaultTag" />).
        /// </param>
        /// <param name="customMapping">
        ///     An optional parameter that provides custom property mappings for the model.
        ///     If provided, these mappings will be used to convert values for properties
        ///     that match the specified logical name and tag.
        /// </param>
        /// <returns>The model with filled properties.</returns>
        public static T FillObisModel<T>(this T model, string logicalName, object value,
            string tag = LogicalNameMappingAttribute.DefaultTag, CustomPropertyMapping<T>? customMapping = null)
            where T : notnull
        {
            var properties = model.GetType().GetProperties();

            foreach (var property in properties)
                if (property.IsDefined(typeof(NestedModelAttribute), false))
                {
                    var nestedModel = CreatePropertyInstanceIfNotExists(model, property.GetValue(model), property);

                    nestedModel = FillObisModel(nestedModel, logicalName, value, tag);
                    property.SetValue(model, nestedModel);
                }
                else
                {
                    var attributes = property
                        .GetCustomAttributes<LogicalNameMappingAttribute>(false)
                        .ToArray();

                    FillPropertiesByAttributes(model, logicalName, value, tag, customMapping, attributes, property);
                }

            return model;
        }

        private static void FillPropertiesByAttributes<TModel>(TModel model, string logicalName, object value,
            string tag, CustomPropertyMapping<TModel>? customMapping, LogicalNameMappingAttribute[] attributes,
            PropertyInfo property) where TModel : notnull
        {
            foreach (var attribute in attributes)
            {
                if (attribute.LogicalName != logicalName || attribute.Tag != tag)
                    continue;
                property.SetValue(model, GetConvertedValue(customMapping, attribute, property, model, value));
            }
        }

        private static object? GetConvertedValue<TModel>(CustomPropertyMapping<TModel>? customMapping,
            LogicalNameMappingAttribute attribute, PropertyInfo property, TModel model, object value)
            where TModel : notnull
        {
            try
            {
                if (customMapping != null && customMapping.Mappings.TryGetValue(attribute, out var mappingDelegate))
                    return mappingDelegate.DynamicInvoke(property.GetValue(model), value);
                return ConvertValue(value, property.PropertyType);
            }
            catch (Exception)
            {
                return attribute.DefaultValue ?? GetDefaultValue(property.PropertyType);
            }
        }

        /// <summary>
        ///     Converts the provided value to the specified target type,
        ///     handling nullable types.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        private static object? ConvertValue(object? value, Type targetType)
        {
            if (!targetType.IsGenericType || targetType.GetGenericTypeDefinition() != typeof(Nullable<>))
                return Convert.ChangeType(value, targetType);
            if (value == null)
                return null;

            var underlyingType = Nullable.GetUnderlyingType(targetType);
            return Convert.ChangeType(value, underlyingType!);
        }

        /// <summary>
        ///     Creates an instance of a property if it is null and returns the instance.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="model">The model that contains the property.</param>
        /// <param name="nestedModel">The current value of the nested model property.</param>
        /// <param name="property">The property to instantiate if null.</param>
        /// <returns>The existing or newly created instance of the nested model.</returns>
        private static object CreatePropertyInstanceIfNotExists<T>(T model, object? nestedModel, PropertyInfo property)
        {
            if (nestedModel != null)
                return nestedModel;
            nestedModel = Activator.CreateInstance(property.PropertyType);
            property.SetValue(model, nestedModel);
            return nestedModel;
        }

        /// <summary>
        ///     Returns the default value for the specified type.
        /// </summary>
        /// <param name="type">The type for which to get the default value.</param>
        /// <returns>The default value for the type.</returns>
        private static object? GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}