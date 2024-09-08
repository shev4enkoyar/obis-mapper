using System;
using ObisMapper.Models;

namespace ObisMapper.FluentMapper.Abstraction
{
    public interface IMappingExpression<TModel>
    {
        IMappingExpression<TModel> ForMember<TDestination>(Func<TModel, TDestination> sourceValue, LogicalNameGroup logicalNameGroup);
        IMappingExpression<TModel> ForMember<TDestination>(Func<TModel, TDestination> sourceValue, LogicalNameGroup logicalNameGroup, Action<IPropertyMappingConfiguration<TModel, TDestination>> configure);
        IMappingExpression<TModel> ForMember<TDestination>(Func<TModel, TDestination> sourceValue, Action<IMappingExpression<TDestination>> nestedConfiguration) where TDestination : class;
    }
}