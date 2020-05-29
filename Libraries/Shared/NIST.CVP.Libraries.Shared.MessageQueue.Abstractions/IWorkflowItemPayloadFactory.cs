using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions
{
	public interface IWorkflowItemPayloadFactory
	{
		IWorkflowItemPayload GetPayload(string payload, APIAction action);
		string SerializePayload(IWorkflowItemPayload payload);
	}
}
