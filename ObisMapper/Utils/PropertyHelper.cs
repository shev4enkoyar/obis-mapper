using System.Reflection;

namespace ObisMapper.Utils
{
    internal static class PropertyHelper
    {
        internal static PropertyInfo? GetPrivateProperty(object type, string name)
        {
            return type.GetType()
                .GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}