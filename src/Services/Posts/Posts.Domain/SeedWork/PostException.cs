using System;

namespace Posts.Domain.SeedWork
{
    public class PostException : Exception
    {
        public PostException() { }

        public PostException(string message) : base(message) { }
        public PostException(string message, Exception innerException) : base(message, innerException) { }
    }
}
