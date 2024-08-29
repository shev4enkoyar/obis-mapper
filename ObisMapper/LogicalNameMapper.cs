using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ObisMapper.Attributes;

namespace ObisMapper
{
    /// <summary>
    ///     Provides functionality to map values to model properties based on logical names,
    ///     with support for nested models and default values.
    /// </summary>
    public class LogicalNameMapper
    {
        private readonly ConcurrentDictionary<PropertyInfo, LogicalNameMappingAttribute[]>
            _logicalNameMappingAttributeCache =
                new ConcurrentDictionary<PropertyInfo, LogicalNameMappingAttribute[]>();

        private readonly ConcurrentDictionary<PropertyInfo, bool> _nestedModelAttributePresenceCache =
            new ConcurrentDictionary<PropertyInfo, bool>();

        private readonly ConcurrentDictionary<Type, PropertyInfo[]> _typePropertiesCache =
            new ConcurrentDictionary<Type, PropertyInfo[]>();
        
        private readonly ConcurrentDictionary<PropertyInfo, Action<object, object?>> _propertySettersCache =
            new ConcurrentDictionary<PropertyInfo, Action<object, object?>>();

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
        public T FillObisModel<T>(T model, string logicalName, object value,
            string tag = LogicalNameMappingAttribute.DefaultTag, CustomPropertyMapping<T>? customMapping = null)
            where T : notnull
        {
            foreach (var property in GetTypeProperties(model.GetType()))
                if (IsNestedModel(property))
                {
                    var nestedModel = CreatePropertyInstanceIfNotExists(model, property.GetValue(model), property);
                    nestedModel = FillObisModel(nestedModel, logicalName, value, tag);
                    SetPropertyValue(model, property, nestedModel);
                }
                else
                {
                    var attributes = GetLogicalNameMappingAttributes(property);

                    FillPropertiesByAttributes(model, logicalName, value, tag, customMapping, attributes, property);
                }

            return model;
        }

        private PropertyInfo[] GetTypeProperties(Type type)
        {
            return _typePropertiesCache.GetOrAdd(type, t => t.GetProperties());
        }

        private bool IsNestedModel(PropertyInfo property)
        {
            return _nestedModelAttributePresenceCache.GetOrAdd(property,
                prop => prop.IsDefined(typeof(NestedModelAttribute), false));
        }

        private LogicalNameMappingAttribute[] GetLogicalNameMappingAttributes(PropertyInfo property)
        {
            return _logicalNameMappingAttributeCache.GetOrAdd(property, prop =>
                prop.GetCustomAttributes<LogicalNameMappingAttribute>(false).ToArray());
        }

        private void FillPropertiesByAttributes<TModel>(TModel model, string logicalName, object value,
            string tag, CustomPropertyMapping<TModel>? customMapping, LogicalNameMappingAttribute[] attributes,
            PropertyInfo property) where TModel : notnull
        {
            foreach (var attribute in attributes)
            {
                if (attribute.LogicalName != logicalName || attribute.Tag != tag)
                    continue;
                SetPropertyValue(model, property, GetConvertedValue(customMapping, attribute, property, model, value));
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
        private object CreatePropertyInstanceIfNotExists<T>(T model, object? nestedModel, PropertyInfo property) where T : notnull
        {
            if (nestedModel != null)
                return nestedModel;
            nestedModel = Activator.CreateInstance(property.PropertyType);
            SetPropertyValue(model, property, nestedModel);
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
        
        private void SetPropertyValue(object model, PropertyInfo property, object? value)
        {
            var setter = _propertySettersCache.GetOrAdd(property, CreateSetterDelegate);
            setter(model, value);
        }

        private static Action<object, object?> CreateSetterDelegate(PropertyInfo property)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var argument = Expression.Parameter(typeof(object), "argument");

            var instanceCast = Expression.Convert(instance, property.DeclaringType!);
            var argumentCast = Expression.Convert(argument, property.PropertyType);

            var setterCall = Expression.Call(instanceCast, property.GetSetMethod()!, argumentCast);

            return Expression.Lambda<Action<object, object?>>(setterCall, instance, argument).Compile();
        }
    }
}