using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Results
{
	public class WorkflowInsertResult : Result
	{
		public long? WorkflowID { get; set; }

		public WorkflowInsertResult() { }
		public WorkflowInsertResult(string errorMessage) : base(errorMessage) { }
	}
}
