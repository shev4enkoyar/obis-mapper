using System;

namespace ObisMapper.Exceptions
{
    public class ExpressionException : Exception
    {
        public ExpressionException(string message) : base(message)
        {
        }

        public ExpressionException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}