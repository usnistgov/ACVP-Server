namespace ACVPWorkflow.Providers
{
	public interface IWorkflowContactProvider
	{
		WorkflowContact GetContactForACVPUser(long acvpUserID);
	}
}