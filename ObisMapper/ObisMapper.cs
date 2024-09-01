using System;
using System.Collections.Generic;
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
        
        public TModel Map<TModel>(TModel model, IEnumerable<ObisDataModel> data,
            AbstractObisModelConverter<TModel> converter) where TModel : IObisModel
        {
            throw new NotImplementedException();
        }

        public async Task<TModel> MapAsync<TModel>(TModel model, IEnumerable<ObisDataModel> data,
            AbstractObisModelConverter<TModel> converter,
            CancellationToken cancellationToken = default) where TModel : IObisModel
        {
            throw new NotImplementedException();
        }
        
        private PropertyInfo[] GetTypeProperties(Type type)
        {
            return _cache.TypePropertiesCache.GetOrAdd(type, t => t.GetProperties());
        }

        // private void ApplyRule<TProperty>(IObisModel model, ConverterRule<TProperty> rule, object dataModel)
        // {
        //     // Получаем значение из dataModel по логическому имени
        //     var value = GetValueFromDataModel(rule, dataModel);
        //
        //     // Если значение не найдено, используем значение по умолчанию
        //     if (value == null) value = rule.DefaultValue;
        //
        //     // Конвертируем значение в нужный тип
        //     var convertedValue = rule.ConversionHandler?.Invoke(value) ?? (TProperty)value;
        //
        //     // Применяем валидацию, если она есть
        //     if (rule.Validator != null && !rule.Validator.ValidationPredicate(convertedValue))
        //         // Если валидация не пройдена, можно выбросить исключение или использовать значение по умолчанию
        //         // throw new Exception(rule.Validator.ValidationMessage);
        //         convertedValue = rule.DefaultValue;
        //
        //     // Устанавливаем значение свойства модели
        //     SetModelPropertyValue(model, rule, convertedValue);
        // }
        //
        // private object GetValueFromDataModel<TProperty>(ConverterRule<TProperty> rule, object dataModel)
        // {
        //     // Здесь нужно реализовать логику для извлечения значения из dataModel по логическому имени
        //     // Например, можно использовать reflection или работать с заранее подготовленным словарем данных
        //     // В зависимости от структуры dataModel
        //     return null; // Этот метод нужно будет реализовать
        // }
        //
        // private void SetModelPropertyValue<TProperty>(IObisModel model, ConverterRule<TProperty> rule, TProperty value)
        // {
        //     // Здесь нужно использовать reflection для установки значения свойства модели
        //     // На основе информации, которую предоставляет ConverterRule
        // }
    }
}