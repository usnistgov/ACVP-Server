using System;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions
{
    public class NotPendingApprovalException : Exception
    {
        public NotPendingApprovalException(string message) : base(message) { }
    }
}