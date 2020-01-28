using ACVPWorkflow.Results;

namespace ACVPWorkflow.Providers
{
	public interface IRequestProvider
	{
		Result Create(long requestID, RequestAction action, long workflowID, long userID);
	}
}