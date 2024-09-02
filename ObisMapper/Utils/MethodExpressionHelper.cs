using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ObisMapper.Utils
{
    internal static class MethodExpressionHelper
    {
        internal static Invoker CreateExpression(Type mainType, Type genericType, string methodName)
        {
            var method = mainType
                .MakeGenericType(genericType)
                .GetMethod(methodName);

            return CreateExpression(method);
        }

        private static Invoker CreateExpression(MethodInfo method)
        {
            var targetArg = Expression.Parameter(typeof(object));
            var argsArg = Expression.Parameter(typeof(object[]));
            Expression body = Expression.Call(method.IsStatic
                ? null
                : Expression.Convert(targetArg, method.DeclaringType), method, method.GetParameters().Select((p, i) =>
                Expression.Convert(Expression.ArrayIndex(argsArg, Expression.Constant(i)), p.ParameterType)));
            if (body.Type == typeof(void))
                body = Expression.Block(body, Expression.Constant(null));
            else if (body.Type.IsValueType)
                body = Expression.Convert(body, typeof(object));
            return Expression.Lambda<Invoker>(body, targetArg, argsArg).Compile();
        }

        internal static InvokerAsync CreateExpressionAsync(Type mainType, Type genericType, string methodName)
        {
            var method = mainType
                .MakeGenericType(genericType)
                .GetMethod(methodName);

            if (method == null) throw new ArgumentException("Method '{0}' not found", methodName);

            return CreateExpressionAsync(method);
        }

        private static InvokerAsync CreateExpressionAsync(MethodInfo method)
        {
            var targetArg = Expression.Parameter(typeof(object));
            var argsArg = Expression.Parameter(typeof(object[]));

            Expression body = Expression.Call(
                Expression.Convert(targetArg, method.DeclaringType),
                method,
                method.GetParameters().Select((p, i) =>
                    Expression.Convert(Expression.ArrayIndex(argsArg, Expression.Constant(i)), p.ParameterType)));

            if (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var resultType = method.ReturnType.GetGenericArguments()[0];
                var taskResult = Expression.Property(body, "Result");
                var taskResultAsObject = Expression.Convert(taskResult, typeof(object));
                body = Expression.Call(
                    typeof(Task),
                    "FromResult",
                    new[] { typeof(object) },
                    taskResultAsObject);
            }
            else if (method.ReturnType == typeof(Task))
            {
                body = Expression.Convert(
                    Expression.Call(typeof(Task), "FromResult", new[] { typeof(object) }, Expression.Constant(null)),
                    typeof(Task<object>));
            }
            else
            {
                body = Expression.Convert(body, typeof(Task<object>));
            }

            return Expression.Lambda<InvokerAsync>(body, targetArg, argsArg).Compile();
        }

        internal delegate object Invoker(object target, params object?[] args);

        internal delegate Task<object> InvokerAsync(object target, params object?[] args);
    }
}