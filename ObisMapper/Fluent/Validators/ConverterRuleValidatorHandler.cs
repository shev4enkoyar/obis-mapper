using System;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Abstractions.Fluent;

namespace ObisMapper.Fluent.Validators
{
    internal class ConverterRuleValidatorHandler<TDestination> : IValidationHandler<TDestination>
    {
        private readonly Func<TDestination, bool> _validationPredicate;

        internal ConverterRuleValidatorHandler(Func<TDestination, bool> validationPredicate, string? validationMessage)
        {
            _validationPredicate = validationPredicate;
            ValidationMessage = validationMessage;
        }

        internal string? ValidationMessage { get; set; }

        public bool Validate(TDestination value)
        {
            return _validationPredicate.Invoke(value);
        }

        public Task<bool> ValidateAsync(TDestination value, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Validate(value));
        }
    }
}