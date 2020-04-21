using System;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models
{
    public class WorkflowItemLite
    {
        public long WorkflowItemId { get; set; }
        public long RequestId { get; set; }
        public APIAction APIAction { get; set; }
        public string SubmissionId { get; set; }
        public string Submitter { get; set; }
        public DateTime Submitted { get; set; }
        public WorkflowStatus Status { get; set; }
    }
}