using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Abstractions.Mappings;

namespace ObisMapper.Abstractions
{
    public interface IObisMapper
    {
        // Default
        TModel Map<TModel>(
            TModel model,
            IEnumerable<ObisDataModel> data,
            AbstractObisModelConverter<TModel> converter) where TModel : IObisModel;

        Task<TModel> MapAsync<TModel>(
            TModel model,
            IEnumerable<ObisDataModel> data,
            AbstractObisModelConverter<TModel> converter,
            CancellationToken cancellationToken = default) where TModel : IObisModel;

        // TModel Map<TModel>(object dataModel, AbstractObisModelConverter<TModel> converter) where TModel : IObisModel;

        // TModel Map<TModel>(object[] dataModels, AbstractObisModelConverter<TModel> converter) where TModel : IObisModel;
    }
}