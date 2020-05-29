using System;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}