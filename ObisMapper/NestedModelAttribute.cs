using System;

namespace ObisMapper
{
    /// <summary>
    ///     Attribute to mark properties as nested models that should be recursively processed
    ///     by the <see cref="LogicalNameMapper.FillObisModel{T}" /> method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NestedModelAttribute : Attribute
    {
    }
}