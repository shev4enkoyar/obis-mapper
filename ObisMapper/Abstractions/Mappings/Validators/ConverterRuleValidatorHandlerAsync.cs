using System;
using System.Threading;
using System.Threading.Tasks;

namespace ObisMapper.Abstractions.Mappings.Validators
{
    internal class ConverterRuleValidatorHandlerAsync<TDestination> : IValidationHandler<TDestination>
    {
        private readonly Func<TDestination, CancellationToken, Task<bool>> _validationPredicate;

        internal ConverterRuleValidatorHandlerAsync(
            Func<TDestination, CancellationToken, Task<bool>> validationPredicate,
            string? validationMessage)
        {
            _validationPredicate = validationPredicate;
            ValidationMessage = validationMessage;
        }

        internal string? ValidationMessage { get; set; }

        public bool Validate(TDestination value)
        {
            return ValidateAsync(value).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<bool> ValidateAsync(TDestination value, CancellationToken cancellationToken = default)
        {
            return await _validationPredicate.Invoke(value, cancellationToken).ConfigureAwait(false);
        }
    }
}