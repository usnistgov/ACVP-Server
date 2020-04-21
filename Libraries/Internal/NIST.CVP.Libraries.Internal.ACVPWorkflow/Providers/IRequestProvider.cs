using NIST.CVP.Libraries.Internal.ACVPWorkflow.Results;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Providers
{
	public interface IRequestProvider
	{
		KillThisResult Create(long requestID, RequestAction action, long workflowID, long userID);
	}
}