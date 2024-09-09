using System;
using System.Linq.Expressions;
using ObisMapper.FluentMapper.Abstraction;
using ObisMapper.Models;

namespace ObisMapper.FluentMapper
{
    internal class NestedMappingExpression<TNestedModel, TParentModel>
        : IMappingNestedExpression<TNestedModel, TParentModel>
    {
        private readonly MappingDictionary _mapping;

        internal NestedMappingExpression(MappingDictionary mapping)
        {
            _mapping = mapping;
        }

        public IMappingExpression<TNestedModel> ForMember<TDestination>(
            Expression<Func<TNestedModel, TDestination>> sourceValue, LogicalNameGroup logicalNameGroup,
            Func<IMappingConfiguration<TNestedModel, TDestination>, IMappingConfiguration<TNestedModel, TDestination>>
                configure)
        {
            var configuration = new MappingConfiguration<TNestedModel, TDestination>();

            if (!(sourceValue.Body is MemberExpression memberExpression))
                return this;

            var propertyHash = memberExpression.Member.GetHashCode();

            var config = configure.Invoke(configuration);
            foreach (var logicalName in logicalNameGroup.LogicalNames)
                _mapping.AddConfiguration(logicalName, propertyHash, config);

            return this;
        }

        public IMappingExpression<TNestedModel> ForMember<TDestination>(Func<TNestedModel, TDestination> sourceValue,
            Action<IMappingNestedExpression<TDestination, TNestedModel>> nestedConfiguration) where TDestination : class
        {
            nestedConfiguration.Invoke(new NestedMappingExpression<TDestination, TNestedModel>(_mapping));
            return this;
        }
    }
}