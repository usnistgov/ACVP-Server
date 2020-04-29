using System;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Exceptions
{
    public class ResourceDoesNotExistException : Exception
    {
        public ResourceDoesNotExistException(string message) : base(message) { }
    }
}