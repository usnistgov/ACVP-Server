using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow
{
	public interface IWorkflowItemPayloadFactory
	{
		IWorkflowItemPayload GetPayload(string payload, APIAction action);
		string SerializePayload(IWorkflowItemPayload payload);
	}
}
