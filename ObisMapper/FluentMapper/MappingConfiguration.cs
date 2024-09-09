using System;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.FluentMapper.Abstraction;

namespace ObisMapper.FluentMapper
{
    internal class MappingConfiguration<TModel, TDestination> : MappingConfigurationBase,
        IMappingConfiguration<TModel, TDestination>
    {
        public void AddConverter(Func<object, TDestination> converter)
        {
        }

        public void AddConverter(Func<object, TDestination, TDestination> converter)
        {
            throw new NotImplementedException();
        }

        public void AddConverterAsync(Func<object, CancellationToken, Task<TDestination>> converter)
        {
            throw new NotImplementedException();
        }

        public void AddConverterAsync(Func<object, TDestination, CancellationToken, Task<TDestination>> converter)
        {
            throw new NotImplementedException();
        }

        public void AddValidator(Func<TDestination, bool> validator)
        {
            throw new NotImplementedException();
        }

        public void AddValidatorAsync(Func<TDestination, CancellationToken, Task<bool>> validator)
        {
            throw new NotImplementedException();
        }

        protected override void Execute()
        {
            throw new NotImplementedException();
        }

        protected override void ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}