using System;
using System.Reflection;
using ObisMapper.Fluent;

namespace ObisMapper.Utils
{
    internal static class ReflectionHelper
    {
        internal static PropertyInfo? GetPrivateProperty(object type, string name)
        {
            return type.GetType()
                .GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        internal static object? GetGenericRule(BaseModelRule modelRule)
        {
            var genericRuleType = typeof(ModelRule<>).MakeGenericType(modelRule.DestinationType);
            return Convert.ChangeType(modelRule, genericRuleType);
        }
    }
}