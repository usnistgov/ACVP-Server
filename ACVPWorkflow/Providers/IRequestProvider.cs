using ACVPWorkflow.Results;

namespace ACVPWorkflow.Providers
{
	public interface IRequestProvider
	{
		KillThisResult Create(long requestID, RequestAction action, long workflowID, long userID);
	}
}