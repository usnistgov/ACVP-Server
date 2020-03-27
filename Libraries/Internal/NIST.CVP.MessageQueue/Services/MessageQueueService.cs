using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.MessageQueue.Providers;

namespace NIST.CVP.MessageQueue.Services
{
	public class MessageQueueService : IMessageQueueService
	{
		private readonly IMessageProvider _messageProvider;

		public MessageQueueService(IMessageProvider messageProvider)
		{
			_messageProvider = messageProvider;
		}

		public void DeleteMessage(Guid id) => _messageProvider.DeleteMessage(id);

		public Message GetNextMessage() => _messageProvider.GetNextMessage();

		public void UpdateMessageStatus(Guid id, MessageStatus messageStatus) => _messageProvider.UpdateStatus(id, messageStatus);
	}
}
