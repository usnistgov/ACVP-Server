using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions
{
	public interface IWorkflowItemPayloadValidator
	{
		public bool Validate(IWorkflowItemPayload workflowItemPayload);
	}
}
