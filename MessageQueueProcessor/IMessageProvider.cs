using System;

namespace MessageQueueProcessor
{
	public interface IMessageProvider
	{
		void DeleteMessage(Guid id);
		Message GetNextMessage();
		void UpdateStatus(Guid id, MessageStatus messageStatus);
	}
}