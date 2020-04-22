using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions
{
	public interface IWorkflowItemPayloadFactory
	{
		IWorkflowItemPayload GetPayload(string payload, APIAction action);
		string SerializePayload(IWorkflowItemPayload payload);
	}
}
