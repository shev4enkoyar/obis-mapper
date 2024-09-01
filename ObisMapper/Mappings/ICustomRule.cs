using System;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Models;

namespace ObisMapper.Mappings
{
    public interface ICustomRule<TDestination>
    {
        ICustomRule<TDestination> AddDefaultValue(TDestination defaultValue);

        ICustomRule<TDestination> AddTag(string tag);

        #region Logical names

        ICustomRule<TDestination> AddLogicalName(LogicalNameModel logicalName);

        ICustomRule<TDestination> AddLogicalName(params LogicalNameModel[] logicalNames);

        ICustomRule<TDestination> AddLogicalName(LogicalNameModelGroup logicalNameGroup);

        #endregion

        #region Converters

        ICustomRule<TDestination> AddConverter(Func<object, TDestination> conversionHandler);

        ICustomRule<TDestination> AddConverter(Func<TDestination, object, TDestination> conversionHandler);

        ICustomRule<TDestination> AddConverter(Func<TDestination, string, object, TDestination> conversionHandler);

        #endregion

        #region Validator

        ICustomRule<TDestination> AddValidator(Func<TDestination, bool> predicate);

        ICustomRule<TDestination> AddValidator(Func<TDestination, bool> predicate, string message);

        ICustomRule<TDestination> AddValidator(Func<TDestination, CancellationToken, Task<bool>> predicate);

        ICustomRule<TDestination> AddValidator(Func<TDestination, CancellationToken, Task<bool>> predicate,
            string message);

        #endregion
    }
}