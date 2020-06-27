using System;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace NIST.CVP.Libraries.Internal.MessageQueue
{
	public class Message
	{
		public Guid ID { get; set; }
		public APIAction APIAction { get; set; }
		public long UserID { get; set; }
		public string Payload { get; set; }
	}
}
