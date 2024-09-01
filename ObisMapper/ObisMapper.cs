using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Abstractions;
using ObisMapper.Abstractions.Mappings;
using ObisMapper.Mappings;

namespace ObisMapper
{
    public class ObisMapper : IObisMapper
    {
        private readonly MappingCache _cache = new MappingCache();

        public TModel Map<TModel>(IEnumerable<ObisDataModel> data, AbstractObisModelConverter<TModel> converter)
            where TModel : IObisModel, new()
        {
            throw new NotImplementedException();
        }

        public async Task<TModel> MapAsync<TModel>(IEnumerable<ObisDataModel> data,
            AbstractObisModelConverter<TModel> converter,
            CancellationToken cancellationToken = default) where TModel : IObisModel
        {
            throw new NotImplementedException();
        }

        public TModel PartialMap<TModel>(TModel model, ObisDataModel data, AbstractObisModelConverter<TModel> converter,
            string tag = "")
            where TModel : IObisModel
        {
            var rules = converter.Rules;

            foreach (var property in GetTypeProperties(model.GetType()))
                if (rules.TryGetValue(property, out var rule))
                {
                    if (!rule.LogicalNameModels.Any(x => x.LogicalName.Equals(data.LogicalName) && x.Tag.Equals(tag)))
                        continue;

                    var genericRule = GetGenericConverterRule(rule);
                    if (genericRule == null)
                        continue;

                    var result = ConvertingStep(model, rule.DestinationType, genericRule, property, data);
                    property.SetValue(model, result);

                    var validationResult = ValidationStep(rule.DestinationType, genericRule, data.Value);
                    if (!validationResult)
                        property.SetValue(model, rule.DefaultValue);
                }

            return model;
        }

        public TModel PartialMapAsync<TModel>(TModel model, ObisDataModel data,
            AbstractObisModelConverter<TModel> converter,
            CancellationToken cancellationToken = default) where TModel : IObisModel
        {
            throw new NotImplementedException();
        }

        private object? GetGenericConverterRule(AbstractRule rule)
        {
            var genericRuleType = typeof(ConverterRule<>).MakeGenericType(rule.DestinationType);
            return Convert.ChangeType(rule, genericRuleType);
        }

        private object ConvertingStep<TModel>(TModel model, Type destinationType, object genericRule,
            PropertyInfo property, ObisDataModel dataModel)
        {
            var conversionHandlerProperty = GetPrivateProperty(genericRule, "ConversionHandler");

            if (conversionHandlerProperty == null)
                return null; // TODO: Use default conversion handler

            var conversionMethod = typeof(IConversionHandler<>)
                .MakeGenericType(destinationType)
                .GetMethod("Convert");

            var conversionInvoker = CreateExpression(conversionMethod);

            var conversionHandler = conversionHandlerProperty.GetValue(genericRule);

            return conversionInvoker.Invoke(conversionHandler, property.GetValue(model), dataModel.Value,
                dataModel.LogicalName);
        }

        private bool ValidationStep(Type destinationType, object genericRule, object value)
        {
            var validationHandlerProperty = GetPrivateProperty(genericRule, "ValidationHandler");

            if (validationHandlerProperty == null) return true;

            var validateMethod = typeof(IValidationHandler<>)
                .MakeGenericType(destinationType)
                .GetMethod("Validate");

            var validationInvoker = CreateExpression(validateMethod);

            var validationHandler = validationHandlerProperty.GetValue(genericRule);

            return (bool)validationInvoker.Invoke(validationHandler, value);
        }

        private PropertyInfo? GetPrivateProperty(object type, string name)
        {
            return type.GetType()
                .GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private PropertyInfo[] GetTypeProperties(Type type)
        {
            return _cache.TypePropertiesCache.GetOrAdd(type, t => t.GetProperties());
        }

        private static Invoker CreateExpression(MethodInfo method)
        {
            var targetArg = Expression.Parameter(typeof(object));
            var argsArg = Expression.Parameter(typeof(object[]));
            Expression body = Expression.Call(
                method.IsStatic ? null : Expression.Convert(targetArg, method.DeclaringType),
                method,
                method.GetParameters().Select((p, i) =>
                    Expression.Convert(Expression.ArrayIndex(argsArg, Expression.Constant(i)), p.ParameterType)));
            if (body.Type == typeof(void))
                body = Expression.Block(body, Expression.Constant(null));
            else if (body.Type.IsValueType)
                body = Expression.Convert(body, typeof(object));
            return Expression.Lambda<Invoker>(body, targetArg, argsArg).Compile();
        }

        private delegate object Invoker(object target, params object[] args);
    }
}