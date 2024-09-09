using System.Threading;

namespace ObisMapper.FluentMapper.Abstraction
{
    internal abstract class MappingConfigurationBase
    {
        protected abstract void Execute();

        protected abstract void ExecuteAsync(CancellationToken cancellationToken);
    }
}