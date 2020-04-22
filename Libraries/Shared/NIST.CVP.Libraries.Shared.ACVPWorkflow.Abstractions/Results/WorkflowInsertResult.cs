using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Results
{
	public class WorkflowInsertResult : Result
	{
		public long? WorkflowID { get; set; }

		public WorkflowInsertResult() { }
		public WorkflowInsertResult(string errorMessage) : base(errorMessage) { }
	}
}
