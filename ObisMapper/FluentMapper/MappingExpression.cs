using System;
using ObisMapper.FluentMapper.Abstraction;
using ObisMapper.Models;

namespace ObisMapper.FluentMapper
{
    public class MappingExpression<TModel> : IMappingExpression<TModel>
    {
        public IMappingExpression<TModel> ForMember<TDestination>(Func<TModel, TDestination> sourceValue, LogicalNameGroup logicalNameGroup)
        {
            throw new NotImplementedException();
        }

        public IMappingExpression<TModel> ForMember<TDestination>(Func<TModel, TDestination> sourceValue, LogicalNameGroup logicalNameGroup, Action<IPropertyMappingConfiguration<TModel, TDestination>> configure)
        {
            throw new NotImplementedException();
        }

        public IMappingExpression<TModel> ForMember<TDestination>(Func<TModel, TDestination> sourceValue, Action<IMappingExpression<TDestination>> nestedConfiguration) where TDestination : class
        {
            throw new NotImplementedException();
        }
    }
}