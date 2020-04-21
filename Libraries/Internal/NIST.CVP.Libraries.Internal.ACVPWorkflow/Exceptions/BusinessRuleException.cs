using System;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}