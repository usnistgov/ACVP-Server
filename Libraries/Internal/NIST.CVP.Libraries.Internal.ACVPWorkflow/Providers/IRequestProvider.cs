using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Results;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Providers
{
	public interface IRequestProvider
	{
		KillThisResult Create(long requestID, RequestAction action, long workflowID, long userID);
	}
}