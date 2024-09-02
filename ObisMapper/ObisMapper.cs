using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Abstractions;
using ObisMapper.Abstractions.Fluent;
using ObisMapper.Constants;
using ObisMapper.Fluent;
using ObisMapper.Fluent.Steps;
using ObisMapper.Utils;

namespace ObisMapper
{
    /// <summary>
    ///     Implements the IObisMapper interface to provide mapping capabilities for OBIS data models.
    /// </summary>
    public class ObisMapper : IObisMapper
    {
        private readonly MappingCache _cache = new MappingCache();

        /// <summary>
        ///     Maps a collection of OBIS data models to a new instance of the specified model type.
        /// </summary>
        public TModel Map<TModel>(IEnumerable<ObisDataModel> data, ModelConfiguration<TModel> configuration,
            string tag = TagConstant.DefaultTag)
            where TModel : IObisModel, new()
        {
            var model = new TModel();
            foreach (var dataModel in data)
                PartialMap(model, dataModel, configuration, tag);

            return model;
        }

        /// <summary>
        ///     Asynchronously maps a collection of OBIS data models to a new instance of the specified model type.
        /// </summary>
        public async Task<TModel> MapAsync<TModel>(IEnumerable<ObisDataModel> data,
            ModelConfiguration<TModel> configuration,
            string tag = TagConstant.DefaultTag,
            CancellationToken cancellationToken = default) where TModel : IObisModel, new()
        {
            var model = new TModel();
            foreach (var dataModel in data)
                await PartialMapAsync(model, dataModel, configuration, tag, cancellationToken);

            return model;
        }

        /// <summary>
        ///     Partially maps data from a single OBIS data model to an existing instance of the specified model type.
        /// </summary>
        public TModel PartialMap<TModel>(TModel model, ObisDataModel data, ModelConfiguration<TModel> configuration,
            string tag = TagConstant.DefaultTag)
            where TModel : IObisModel
        {
            foreach (var property in GetTypeProperties(model.GetType()))
                if (configuration.Rules.TryGetValue(property, out var rule))
                {
                    if (!AdditionalStep.CheckIsDataAccessAllowed(rule, data.LogicalName, tag))
                        continue;

                    var genericRule = ReflectionHelper.GetGenericRule(rule);
                    if (genericRule == null)
                        continue;

                    var conversionResult = ConversionProcess(model, rule, genericRule, property, data);

                    ValidationProcess(model, rule, genericRule, property, conversionResult);
                }

            return model;
        }

        /// <summary>
        ///     Asynchronously partially maps data from a single OBIS data model to an existing instance of the specified model
        ///     type.
        /// </summary>
        public async Task<TModel> PartialMapAsync<TModel>(TModel model, ObisDataModel data,
            ModelConfiguration<TModel> configuration, string tag = TagConstant.DefaultTag,
            CancellationToken cancellationToken = default) where TModel : IObisModel
        {
            foreach (var property in GetTypeProperties(model.GetType()))
                if (configuration.Rules.TryGetValue(property, out var rule))
                {
                    if (!AdditionalStep.CheckIsDataAccessAllowed(rule, data.LogicalName, tag))
                        continue;

                    var genericRule = ReflectionHelper.GetGenericRule(rule);
                    if (genericRule == null)
                        continue;

                    var conversionResult = await ConversionProcessAsync(model, rule, genericRule, property, data,
                        cancellationToken);

                    await ValidationProcessAsync(model, rule, genericRule, property, conversionResult,
                        cancellationToken);
                }

            return model;
        }

        private static object? ConversionProcess<TModel>(TModel model,
            BaseModelRule rule,
            object genericRule,
            PropertyInfo property,
            ObisDataModel data)
        {
            var result = ConversionStep.Process(model, rule, genericRule, property, data);
            property.SetValue(model, result);
            return result;
        }

        private static void ValidationProcess<TModel>(TModel model,
            BaseModelRule rule,
            object genericRule,
            PropertyInfo property,
            object? value)
        {
            var validationResult = ValidationStep.Process(rule.DestinationType, genericRule, value);
            if (!validationResult)
                property.SetValue(model, rule.DefaultValue);
        }

        private static async Task<object?> ConversionProcessAsync<TModel>(TModel model,
            BaseModelRule rule,
            object genericRule,
            PropertyInfo property,
            ObisDataModel data,
            CancellationToken cancellationToken)
        {
            var result = await ConversionStep.ProcessAsync(model, rule, genericRule, property, data,
                cancellationToken);
            property.SetValue(model, result);
            return result;
        }

        private static async Task ValidationProcessAsync<TModel>(TModel model,
            BaseModelRule rule,
            object genericRule,
            PropertyInfo property,
            object? value,
            CancellationToken cancellationToken)
        {
            var validationResult = await ValidationStep.ProcessAsync(rule.DestinationType, genericRule, value,
                cancellationToken);
            if (!validationResult)
                property.SetValue(model, rule.DefaultValue);
        }

        private PropertyInfo[] GetTypeProperties(Type type)
        {
            return _cache.TypePropertiesCache.GetOrAdd(type, t => t.GetProperties());
        }
    }
}