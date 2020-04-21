using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.ACVPWorkflow;

namespace MessageQueueProcessor
{
	public class MessageQueueProcessorConfig
	{
		public int SleepDuration { get; set; }
		public Dictionary<APIAction, bool> AutoApprove { get; set; }
	}
}
