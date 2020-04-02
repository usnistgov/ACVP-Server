using System.Collections.Generic;
using ACVPWorkflow;

namespace MessageQueueProcessor
{
	public class MessageQueueProcessorConfig
	{
		public int SleepDuration { get; set; }
		public Dictionary<APIAction, bool> AutoApprove { get; set; }
	}
}
