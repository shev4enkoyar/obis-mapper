using System;
using System.Linq.Expressions;
using ObisMapper.Models;

namespace ObisMapper.FluentMapper.Abstraction
{
    public interface IMappingExpression<TModel>
    {
        IMappingExpression<TModel> ForMember<TDestination>(Expression<Func<TModel, TDestination>> sourceValue,
            LogicalNameGroup logicalNameGroup);

        IMappingExpression<TModel> ForMember<TDestination>(Expression<Func<TModel, TDestination>> sourceValue,
            LogicalNameGroup logicalNameGroup,
            Func<IMappingConfiguration<TModel, TDestination>, IMappingConfiguration<TModel, TDestination>> configure);

        IMappingExpression<TModel> ForMember<TDestination>(Func<TModel, TDestination> sourceValue,
            Action<IMappingNestedExpression<TDestination, TModel>> nestedConfiguration) where TDestination : class;
    }
}