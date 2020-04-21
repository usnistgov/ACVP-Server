using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Results;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Providers
{
	public interface IRequestProvider
	{
		KillThisResult Create(long requestID, RequestAction action, long workflowID, long userID);
	}
}