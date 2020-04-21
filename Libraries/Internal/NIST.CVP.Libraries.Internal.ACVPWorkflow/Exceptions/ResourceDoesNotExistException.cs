using System;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions
{
    public class ResourceDoesNotExistException : Exception
    {
        public ResourceDoesNotExistException(string message) : base(message) { }
    }
}