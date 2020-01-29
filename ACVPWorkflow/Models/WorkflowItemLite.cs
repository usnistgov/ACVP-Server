using System;

namespace ACVPWorkflow.Models
{
    public class WorkflowItemLite
    {
        public long WorkflowItemId { get; set; }
        public WorkflowItemType WorkflowItemType { get; set; }
        public WorkflowStatus WorkflowStatus { get; set; }
        public string SubmissionId { get; set; }
        public string Submitter { get; set; }
        public DateTime Submitted { get; set; }
    }
}