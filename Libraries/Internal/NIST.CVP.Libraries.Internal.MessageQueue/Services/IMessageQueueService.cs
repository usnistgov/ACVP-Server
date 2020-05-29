using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Libraries.Internal.MessageQueue.Services
{
	public interface IMessageQueueService
	{
		void DeleteMessage(Guid id);
		Message GetNextMessage();
		void UpdateMessageStatus(Guid id, MessageStatus messageStatus);
		List<MessageQueueItem> List();
		string GetMessagePayload(Guid id);
	}
}
