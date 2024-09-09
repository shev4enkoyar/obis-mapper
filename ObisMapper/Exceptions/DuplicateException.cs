using System;

namespace ObisMapper.Exceptions
{
    public class DuplicateException : ArgumentException
    {
        public DuplicateException(string message, string argumentsWithSplitter) : base(message, argumentsWithSplitter)
        {
        }
    }
}