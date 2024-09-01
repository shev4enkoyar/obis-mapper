using System.Threading;
using System.Threading.Tasks;

namespace ObisMapper.Abstractions.Mappings.Validators
{
    public interface IValidationHandler<TDestination>
    {
        bool Validate(TDestination value);

        Task<bool> ValidateAsync(TDestination value, CancellationToken cancellationToken = default);
    }
}