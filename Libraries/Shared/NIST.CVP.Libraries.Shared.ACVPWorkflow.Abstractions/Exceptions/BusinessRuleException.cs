using System;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}