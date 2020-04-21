using System;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Exceptions
{
    public class NotPendingApprovalException : Exception
    {
        public NotPendingApprovalException(string message) : base(message) { }
    }
}