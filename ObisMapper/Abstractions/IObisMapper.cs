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
            IEnumerable<ObisDataModel> data,
            AbstractObisModelConverter<TModel> converter) where TModel : IObisModel, new();

        Task<TModel> MapAsync<TModel>(
            IEnumerable<ObisDataModel> data,
            AbstractObisModelConverter<TModel> converter,
            CancellationToken cancellationToken = default) where TModel : IObisModel;

        TModel PartialMap<TModel>(
            TModel model,
            ObisDataModel data,
            AbstractObisModelConverter<TModel> converter, string tag = "") where TModel : IObisModel;

        TModel PartialMapAsync<TModel>(
            TModel model,
            ObisDataModel data,
            AbstractObisModelConverter<TModel> converter,
            CancellationToken cancellationToken = default) where TModel : IObisModel;

        // TModel Map<TModel>(object dataModel, AbstractObisModelConverter<TModel> converter) where TModel : IObisModel;

        // TModel Map<TModel>(object[] dataModels, AbstractObisModelConverter<TModel> converter) where TModel : IObisModel;
    }
}