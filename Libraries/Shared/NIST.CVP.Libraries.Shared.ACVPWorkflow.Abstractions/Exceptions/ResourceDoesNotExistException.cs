using System;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Exceptions
{
    public class ResourceDoesNotExistException : Exception
    {
        public ResourceDoesNotExistException(string message) : base(message) { }
    }
}