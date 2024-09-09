using System.Collections.Generic;
using ObisMapper.Constants;
using ObisMapper.Exceptions;
using ObisMapper.FluentMapper.Abstraction;
using ObisMapper.Models;

namespace ObisMapper.FluentMapper
{
    internal class MappingDictionary
    {
        private readonly Dictionary<LogicalName, IMappingConfiguration[]> _cachedConfigurations =
            new Dictionary<LogicalName, IMappingConfiguration[]>();

        private readonly Dictionary<string, Dictionary<int, Dictionary<string, IMappingConfiguration>>> _mapping =
            new Dictionary<string, Dictionary<int, Dictionary<string, IMappingConfiguration>>>();


        internal void AddConfiguration(LogicalName logicalName, int propertyMemberHash,
            IMappingConfiguration configuration)
        {
            if (_mapping.TryGetValue(logicalName.Name, out var propertyLevelMapping))
            {
                if (propertyLevelMapping.TryGetValue(propertyMemberHash, out var tagLevelMapping))
                {
                    if (!tagLevelMapping.TryAdd(logicalName.Tag, configuration))
                        throw new DuplicateException("Configuration for this arguments already exists.",
                            $"{nameof(logicalName.Name)}:{nameof(propertyMemberHash)}:{nameof(logicalName.Tag)}");
                }
                else
                {
                    propertyLevelMapping.Add(propertyMemberHash, new Dictionary<string, IMappingConfiguration>
                    {
                        { logicalName.Tag, configuration }
                    });
                }
            }
            else
            {
                _mapping.Add(logicalName.Name, new Dictionary<int, Dictionary<string, IMappingConfiguration>>
                {
                    {
                        propertyMemberHash, new Dictionary<string, IMappingConfiguration>
                        {
                            { logicalName.Tag, configuration }
                        }
                    }
                });
            }
        }

        internal IMappingConfiguration[] GetConfigurations(LogicalName logicalName)
        {
            if (_cachedConfigurations.TryGetValue(logicalName, out var cachedConfigurations))
                return cachedConfigurations;

            var configurations = new List<IMappingConfiguration>();
            if (!_mapping.TryGetValue(logicalName.Name, out var propertyLevelMappings))
            {
                _cachedConfigurations.Add(logicalName, configurations.ToArray());
                return configurations.ToArray();
            }

            foreach (var (_, tagLevelMapping) in propertyLevelMappings)
                if (tagLevelMapping.TryGetValue(logicalName.Tag, out var configuration))
                {
                    configurations.Add(configuration);
                }
                else
                {
                    if (tagLevelMapping.TryGetValue(TagConstant.DefaultTag, out var defaultConfiguration))
                        configurations.Add(defaultConfiguration);
                }

            var configurationArray = configurations.ToArray();

            _cachedConfigurations.Add(logicalName, configurationArray);
            return configurationArray;
        }
    }
}