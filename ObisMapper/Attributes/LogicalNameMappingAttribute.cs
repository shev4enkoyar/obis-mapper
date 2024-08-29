using System;

namespace ObisMapper.Attributes
{
    /// <summary>
    ///     Attribute to map properties to specific logical names with an optional tag and default value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class LogicalNameMappingAttribute : Attribute
    {
        /// <summary>
        ///     The default tag value.
        /// </summary>
        public const string DefaultTag = "default";

        /// <summary>
        ///     Initializes a new instance of the <see cref="LogicalNameMappingAttribute" /> class.
        /// </summary>
        /// <param name="logicalName">The logical name to map to the property.</param>
        public LogicalNameMappingAttribute(string logicalName)
        {
            LogicalName = logicalName;
        }

        /// <summary>
        ///     Gets or sets the tag to distinguish different mappings for the same logical name.
        /// </summary>
        public string Tag { get; set; } = DefaultTag;

        /// <summary>
        ///     Gets the logical name that the property is mapped to.
        /// </summary>
        public string LogicalName { get; }

        /// <summary>
        ///     Gets or sets the default value to use if the conversion of the value fails.
        /// </summary>
        public object? DefaultValue { get; set; }
    }
}