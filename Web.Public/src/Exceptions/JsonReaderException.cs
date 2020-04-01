using System;

namespace Web.Public.Exceptions
{
    public class JsonReaderException : Exception
    {
        public JsonReaderException(string message) : base(message) { }
    }
}