using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Abstractions;
using ObisMapper.Abstractions.Fluent;
using ObisMapper.Fluent;
using ObisMapper.Fluent.Steps;

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
            string tag = "")
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
            string tag = "",
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
            string tag = "")
            where TModel : IObisModel
        {
            foreach (var property in GetTypeProperties(model.GetType()))
                if (configuration.Rules.TryGetValue(property, out var rule))
                {
                    // TODO: Check another rules for obis code with tag and if not exists use primary
                    if (!ValidateRuleDataAccess(rule, data.LogicalName, tag))
                        continue;

                    var genericRule = GetGenericRule(rule);
                    if (genericRule == null)
                        continue;

                    var result = ConversionStep.Process(model, rule, genericRule, property, data);
                    property.SetValue(model, result);

                    var validationResult = ValidationStep.Process(rule.DestinationType, genericRule, result);
                    if (!validationResult)
                        property.SetValue(model, rule.DefaultValue);
                }

            return model;
        }

        /// <summary>
        ///     Asynchronously partially maps data from a single OBIS data model to an existing instance of the specified model
        ///     type.
        /// </summary>
        public async Task<TModel> PartialMapAsync<TModel>(TModel model, ObisDataModel data,
            ModelConfiguration<TModel> configuration, string tag = "",
            CancellationToken cancellationToken = default) where TModel : IObisModel
        {
            foreach (var property in GetTypeProperties(model.GetType()))
                if (configuration.Rules.TryGetValue(property, out var rule))
                {
                    // TODO: Check another rules for obis code with tag and if not exists use primary
                    if (!ValidateRuleDataAccess(rule, data.LogicalName, tag))
                        continue;

                    var genericRule = GetGenericRule(rule);
                    if (genericRule == null)
                        continue;

                    var conversionResult =
                        await ConversionStep.ProcessAsync(model, rule, genericRule, property, data, cancellationToken);
                    property.SetValue(model, conversionResult);

                    var validationResult = await ValidationStep.ProcessAsync(rule.DestinationType, genericRule,
                        conversionResult, cancellationToken);
                    if (!validationResult)
                        property.SetValue(model, rule.DefaultValue);
                }

            return model;
        }

        // TODO: Hmmm... Not good enough
        private static bool ValidateRuleDataAccess(BaseModelRule modelRule, string logicalName, string tag)
        {
            if (modelRule.LogicalNameModels.Any(x => x.LogicalName.Equals(logicalName)
                                                     && (x.Tag.Equals(tag) || modelRule.IsPrimary)))
                return true;

            return false;
        }

        private static object? GetGenericRule(BaseModelRule modelRule)
        {
            var genericRuleType = typeof(ModelRule<>).MakeGenericType(modelRule.DestinationType);
            return Convert.ChangeType(modelRule, genericRuleType);
        }

        private PropertyInfo[] GetTypeProperties(Type type)
        {
            return _cache.TypePropertiesCache.GetOrAdd(type, t => t.GetProperties());
        }
    }
}