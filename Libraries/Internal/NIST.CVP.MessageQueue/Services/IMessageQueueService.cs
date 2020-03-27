using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.MessageQueue.Services
{
	public interface IMessageQueueService
	{
		void DeleteMessage(Guid id);
		Message GetNextMessage();
		void UpdateMessageStatus(Guid id, MessageStatus messageStatus);
	}
}
