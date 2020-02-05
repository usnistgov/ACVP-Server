using System;

namespace ACVPWorkflow.Exceptions
{
    /// <summary>
    /// Throw when Workflow item processors are unsuccessful in creating the requested resource
    /// </summary>
    public class ResourceProcessorException : Exception
    {
        public ResourceProcessorException(string message) : base(message)
        {
        }
    }
}