using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Constants;
using ObisMapper.Fluent;

namespace ObisMapper.Abstractions.Fluent
{
    /// <summary>
    ///     Provides methods for mapping OBIS data models to strongly-typed model objects.
    /// </summary>
    public interface IObisMapper
    {
        /// <summary>
        ///     Maps a collection of OBIS data models to a new instance of the specified model type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to map to.</typeparam>
        /// <param name="data">The collection of OBIS data models to map from.</param>
        /// <param name="configuration">The configuration that defines the mapping rules.</param>
        /// <param name="tag">An optional tag to filter specific data.</param>
        /// <returns>A new instance of <typeparamref name="TModel" /> with mapped data.</returns>
        TModel Map<TModel>(
            IEnumerable<ObisDataModel> data,
            ModelConfiguration<TModel> configuration,
            string tag = TagConstant.DefaultTag) where TModel : IObisModel, new();

        /// <summary>
        ///     Asynchronously maps a collection of OBIS data models to a new instance of the specified model type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to map to.</typeparam>
        /// <param name="data">The collection of OBIS data models to map from.</param>
        /// <param name="configuration">The configuration that defines the mapping rules.</param>
        /// <param name="tag">An optional tag to filter specific data.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains a new instance of
        ///     <typeparamref name="TModel" /> with mapped data.
        /// </returns>
        Task<TModel> MapAsync<TModel>(
            IEnumerable<ObisDataModel> data,
            ModelConfiguration<TModel> configuration,
            string tag = TagConstant.DefaultTag,
            CancellationToken cancellationToken = default) where TModel : IObisModel, new();

        /// <summary>
        ///     Partially maps data from a single OBIS data model to an existing instance of the specified model type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to map to.</typeparam>
        /// <param name="model">The existing model instance to map data to.</param>
        /// <param name="data">The OBIS data model to map from.</param>
        /// <param name="configuration">The configuration that defines the mapping rules.</param>
        /// <param name="tag">An optional tag to filter specific data.</param>
        /// <returns>The <paramref name="model" /> instance with updated data.</returns>
        TModel PartialMap<TModel>(
            TModel model,
            ObisDataModel data,
            ModelConfiguration<TModel> configuration,
            string tag = TagConstant.DefaultTag) where TModel : IObisModel;

        /// <summary>
        ///     Asynchronously partially maps data from a single OBIS data model to an existing instance of the specified model
        ///     type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to map to.</typeparam>
        /// <param name="model">The existing model instance to map data to.</param>
        /// <param name="data">The OBIS data model to map from.</param>
        /// <param name="configuration">The configuration that defines the mapping rules.</param>
        /// <param name="tag">An optional tag to filter specific data.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains the <paramref name="model" />
        ///     instance with updated data.
        /// </returns>
        Task<TModel> PartialMapAsync<TModel>(
            TModel model,
            ObisDataModel data,
            ModelConfiguration<TModel> configuration,
            string tag = TagConstant.DefaultTag,
            CancellationToken cancellationToken = default) where TModel : IObisModel;
    }
}