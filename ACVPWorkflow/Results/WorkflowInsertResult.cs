namespace ACVPWorkflow.Results
{
	public class WorkflowInsertResult : Result
	{
		public long? WorkflowID { get; set; }

		public WorkflowInsertResult() { }
		public WorkflowInsertResult(string errorMessage) : base(errorMessage) { }
	}
}
