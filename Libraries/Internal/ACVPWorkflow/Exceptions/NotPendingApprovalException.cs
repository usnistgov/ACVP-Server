using System;

namespace ACVPWorkflow.Exceptions
{
    public class NotPendingApprovalException : Exception
    {
        public NotPendingApprovalException(string message) : base(message) { }
    }
}