using System.Collections.Generic;
using ACVPWorkflow;

namespace MessageQueueProcessor
{
	public interface IAutoApproveProvider
	{
		Dictionary<APIAction, bool> GetAutoApproveConfiguration();
	}
}