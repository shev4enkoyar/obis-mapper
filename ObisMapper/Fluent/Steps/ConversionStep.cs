using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Abstractions;
using ObisMapper.Abstractions.Fluent;
using ObisMapper.Utils;

namespace ObisMapper.Fluent.Steps
{
    internal static class ConversionStep
    {
        internal static object? Process<TModel>(TModel model, BaseModelRule modelRule, object genericRule,
            PropertyInfo property, ObisDataModel dataModel)
        {
            var conversionHandlerProperty = ReflectionHelper.GetPrivateProperty(genericRule, "ConversionHandler");

            var conversionHandler = conversionHandlerProperty?.GetValue(genericRule);
            if (conversionHandler == null)
                return DefaultValueConverter(modelRule.DestinationType, dataModel.Value, modelRule.DefaultValue);

            var conversionInvoker =
                MethodExpressionHelper.CreateExpression(typeof(IConversionHandler<>), modelRule.DestinationType,
                    "Convert");

            try
            {
                return conversionInvoker.Invoke(conversionHandler, property.GetValue(model), dataModel.Value,
                    dataModel.LogicalName);
            }
            catch (Exception)
            {
                return DefaultValueConverter(modelRule.DestinationType, dataModel.Value, modelRule.DefaultValue);
            }
        }

        internal static async Task<object?> ProcessAsync<TModel>(TModel model, BaseModelRule modelRule,
            object genericRule, PropertyInfo property, ObisDataModel dataModel, CancellationToken cancellationToken)
        {
            var conversionHandlerProperty = ReflectionHelper.GetPrivateProperty(genericRule, "ConversionHandler");

            var conversionHandler = conversionHandlerProperty?.GetValue(genericRule);
            if (conversionHandler == null)
                return DefaultValueConverter(modelRule.DestinationType, dataModel.Value, modelRule.DefaultValue);

            var conversionInvokerAsync =
                MethodExpressionHelper.CreateExpressionAsync(typeof(IConversionHandler<>), modelRule.DestinationType,
                    "ConvertAsync");

            try
            {
                return await conversionInvokerAsync.Invoke(conversionHandler, property.GetValue(model), dataModel.Value,
                    dataModel.LogicalName, cancellationToken);
            }
            catch (Exception)
            {
                return DefaultValueConverter(modelRule.DestinationType, dataModel.Value, modelRule.DefaultValue);
            }
        }

        private static object? DefaultValueConverter(Type destinationType, object? value, object? defaultValue)
        {
            if (value != null && value.GetType() == destinationType) return value;

            if (value == null) return defaultValue;

            var convertedValue = Convert.ChangeType(value, destinationType);
            return convertedValue ?? defaultValue;
        }
    }
}