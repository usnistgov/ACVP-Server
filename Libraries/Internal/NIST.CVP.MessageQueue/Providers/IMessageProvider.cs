using System;

namespace NIST.CVP.MessageQueue.Providers
{
	public interface IMessageProvider
	{
		void DeleteMessage(Guid id);
		Message GetNextMessage();
		void UpdateStatus(Guid id, MessageStatus messageStatus);
	}
}