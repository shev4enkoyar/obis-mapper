using System;
using System.Collections.Concurrent;
using System.Reflection;
using ObisMapper.Attributes;

namespace ObisMapper
{
    internal class MappingCache
    {
        internal readonly ConcurrentDictionary<PropertyInfo, LogicalNameMappingAttribute[]>
            LogicalNameMappingAttributeCache =
                new ConcurrentDictionary<PropertyInfo, LogicalNameMappingAttribute[]>();

        internal readonly ConcurrentDictionary<PropertyInfo, bool> NestedModelAttributePresenceCache =
            new ConcurrentDictionary<PropertyInfo, bool>();

        internal readonly ConcurrentDictionary<PropertyInfo, Action<object, object?>> PropertySettersCache =
            new ConcurrentDictionary<PropertyInfo, Action<object, object?>>();

        internal readonly ConcurrentDictionary<Type, PropertyInfo[]> TypePropertiesCache =
            new ConcurrentDictionary<Type, PropertyInfo[]>();
    }
}