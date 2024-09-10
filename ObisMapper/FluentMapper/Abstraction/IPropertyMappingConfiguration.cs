using System;
using System.Threading;
using System.Threading.Tasks;

namespace ObisMapper.FluentMapper.Abstraction
{
    public interface IMappingConfiguration
    {
    }

    public interface IMappingConfiguration<TModel, TDestination> : IMappingConfiguration
    {
        void SetDefaultValue(TDestination value);
        void AddConverter(Func<object, TDestination> converter);

        void AddConverter(Func<object, TDestination, TDestination> converter);

        void AddConverterAsync(Func<object, CancellationToken, Task<TDestination>> converter);

        void AddConverterAsync(Func<object, TDestination, CancellationToken, Task<TDestination>> converter);

        void AddValidator(Func<TDestination, bool> validator);

        void AddValidatorAsync(Func<TDestination, CancellationToken, Task<bool>> validator);
    }
}