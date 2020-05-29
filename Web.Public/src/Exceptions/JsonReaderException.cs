using System;
using System.Collections.Generic;

namespace Web.Public.Exceptions
{
    public class JsonReaderException : Exception
    {
        public List<string> Errors { get; } = new List<string>();

        public JsonReaderException(string message) : base(message)
        {
            Errors.Add(message);
        }

        public JsonReaderException(List<string> errors)
        {
            Errors = errors;
        }
    }
}