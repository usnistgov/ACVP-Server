using System;
using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.MessageQueue.Providers
{
	public interface IMessageQueueProvider
	{
		void DeleteMessage(Guid id);
		Message GetNextMessage();
		void UpdateStatus(Guid id, MessageStatus messageStatus);
		List<MessageQueueItem> List();
		string GetMessagePayload(Guid id);
	}
}