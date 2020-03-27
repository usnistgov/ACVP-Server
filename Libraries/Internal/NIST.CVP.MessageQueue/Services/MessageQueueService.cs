using System;
using System.Collections.Generic;
using NIST.CVP.MessageQueue.Providers;

namespace NIST.CVP.MessageQueue.Services
{
	public class MessageQueueService : IMessageQueueService
	{
		private readonly IMessageQueueProvider _messageProvider;

		public MessageQueueService(IMessageQueueProvider messageProvider)
		{
			_messageProvider = messageProvider;
		}

		public void DeleteMessage(Guid id) => _messageProvider.DeleteMessage(id);

		public Message GetNextMessage() => _messageProvider.GetNextMessage();

		public void UpdateMessageStatus(Guid id, MessageStatus messageStatus) => _messageProvider.UpdateStatus(id, messageStatus);

		public List<MessageQueueItem> List() => _messageProvider.List();

		public string GetMessagePayload(Guid id) => _messageProvider.GetMessagePayload(id);
	}
}
