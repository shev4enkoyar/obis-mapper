using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using ObisMapper.Abstractions.Fluent;
using ObisMapper.Utils;

namespace ObisMapper.Fluent.Steps
{
    internal static class ValidationStep
    {
        internal static bool Process(Type destinationType, object genericRule, object? value)
        {
            var validationHandlerProperty = PropertyHelper.GetPrivateProperty(genericRule, "ValidationHandlers");

            var validationHandlers = validationHandlerProperty?.GetValue(genericRule);
            if (validationHandlers == null)
                return true;

            foreach (var handler in (IEnumerable)validationHandlers)
            {
                var validationInvoker =
                    MethodExpressionHelper.CreateExpression(typeof(IValidationHandler<>), destinationType, "Validate");
                var validationResult = (bool)validationInvoker.Invoke(handler, value);
                if (!validationResult) return false;
            }

            return true;
        }

        internal static async Task<bool> ProcessAsync(Type destinationType, object genericRule, object? value,
            CancellationToken cancellationToken)
        {
            var validationHandlerProperty = PropertyHelper.GetPrivateProperty(genericRule, "ValidationHandlers");

            var validationHandlers = validationHandlerProperty?.GetValue(genericRule);
            if (validationHandlers == null)
                return true;

            foreach (var handler in (IEnumerable)validationHandlers)
            {
                var validationInvoker =
                    MethodExpressionHelper.CreateExpressionAsync(typeof(IValidationHandler<>), destinationType,
                        "ValidateAsync");
                var validationResult = (bool)await validationInvoker.Invoke(handler, value, cancellationToken);
                if (!validationResult) return false;
            }

            return true;
        }
    }
}