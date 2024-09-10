using System;
using System.Linq.Expressions;
using ObisMapper.FluentMapper.Abstraction;
using ObisMapper.Models;

namespace ObisMapper.FluentMapper
{
    internal class MappingExpression<TModel> : IMappingExpression<TModel>
    {
        private readonly MappingDictionary _mapping = new MappingDictionary();

        public IMappingExpression<TModel> ForMember<TDestination>(Expression<Func<TModel, TDestination>> sourceValue,
            LogicalNameGroup logicalNameGroup,
            Func<IMappingConfiguration<TModel, TDestination>, IMappingConfiguration<TModel, TDestination>> configure)
        {
            if (!(sourceValue.Body is MemberExpression memberExpression))
                return this;

            var propertyHash = memberExpression.Member.GetHashCode();

            var config = configure.Invoke(new MappingConfiguration<TModel, TDestination>());
            foreach (var logicalName in logicalNameGroup.LogicalNames)
                _mapping.AddConfiguration(logicalName, propertyHash, config);

            return this;
        }

        public IMappingExpression<TModel> ForMember<TDestination>(Func<TModel, TDestination> sourceValue,
            Action<IMappingNestedExpression<TDestination, TModel>> nestedConfiguration) where TDestination : class
        {
            nestedConfiguration.Invoke(new NestedMappingExpression<TDestination, TModel>(_mapping));
            return this;
        }
    }
}