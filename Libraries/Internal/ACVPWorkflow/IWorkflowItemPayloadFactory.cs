using System.Text.Json;
using ACVPWorkflow.Models;

namespace ACVPWorkflow
{
	public interface IWorkflowItemPayloadFactory
	{
		IWorkflowItemPayload GetPayload(string payload, APIAction action);
		string SerializePayload(IWorkflowItemPayload payload);
	}
}
