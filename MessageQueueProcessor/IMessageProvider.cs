using System;

namespace MessageQueueProcessor
{
	public interface IMessageProvider
	{
		void DeleteMessage(Guid id);
		Message GetNextMessage();
		void MarkForJavaProcessor(Guid id);
	}
}