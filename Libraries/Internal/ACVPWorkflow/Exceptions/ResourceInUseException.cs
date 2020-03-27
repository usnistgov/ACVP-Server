using System;

namespace ACVPWorkflow.Exceptions
{
    /// <summary>
    /// Thrown when a delete resource is attempted,
    /// but cannot be completed because it is being referenced by other resources.
    /// </summary>
    public class ResourceInUseException : Exception
    {
        public ResourceInUseException(string message) : base(message) { }
    }
}