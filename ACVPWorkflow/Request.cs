namespace ACVPWorkflow
{
	public class Request
	{
		public long? ID { get; set; }
		public RequestAction Action { get; set; }
		public long WorkflowID { get; set; }
		public long UserID { get; set; }

	}
}
