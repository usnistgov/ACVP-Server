using System;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace NIST.CVP.Libraries.Internal.MessageQueue
{
	public class MessageQueueItem
	{
		public Guid ID { get; set; }
		public MessageStatus Status { get; set; }
		public APIAction APIAction { get; set; }
		public int Length { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
