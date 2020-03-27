using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.MessageQueue
{
	public class MessageQueueItem
	{
		public Guid ID { get; set; }
		public MessageStatus Status { get; set; }
		public MessageType MessageType { get; set; }
		public int Length { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
