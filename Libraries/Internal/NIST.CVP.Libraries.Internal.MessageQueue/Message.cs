using NIST.CVP.Libraries.Internal.ACVPWorkflow;
using System;
using System.Text.Json;
using NIST.CVP.Libraries.Internal.MessageQueue.MessagePayloads;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.MessageQueue
{
	public class Message
	{
		public Guid ID { get; set; }
		public APIAction MessageType { get; set; }
		public long UserID { get; set; }
		public string Payload { get; set; }
	}
}
