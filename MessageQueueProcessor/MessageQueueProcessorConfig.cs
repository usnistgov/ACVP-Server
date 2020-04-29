using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.ACVPWorkflow;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace MessageQueueProcessor
{
	public class MessageQueueProcessorConfig
	{
		public int SleepDuration { get; set; }
		public Dictionary<APIAction, bool> AutoApprove { get; set; }
	}
}
