using System;
using System.Collections.Generic;
using System.Linq;
using CVP.DatabaseInterface;
using Mighty;

namespace NIST.CVP.MessageQueue.Providers
{
	public class MessageQueueProvider : IMessageQueueProvider
	{
		private readonly string _acvpConnectionString;

		public MessageQueueProvider(IConnectionStringFactory connectionStringFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
		}

		public Message GetNextMessage()
		{
			var db = new MightyOrm<Message>(_acvpConnectionString);

			return db.SingleFromProcedure("common.MessageQueueGetNext");
		}

		public void UpdateStatus(Guid id, MessageStatus messageStatus)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.ExecuteProcedure("common.MessageQueueUpdateStatus", inParams: new
			{
				MessageId = id,
				StatusId = messageStatus
			});
		}

		public void DeleteMessage(Guid id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.ExecuteProcedure("common.MessageQueueDelete", inParams: new
			{
				MessageId = id
			});
		}

		public List<MessageQueueItem> List()
		{
			var db = new MightyOrm<MessageQueueItem>(_acvpConnectionString);

			return db.QueryFromProcedure("common.MessageQueueList").ToList();
		}

		public string GetMessagePayload(Guid id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			return (string)db.ScalarFromProcedure("common.MessageGetPayload", inParams: new
			{
				MessageId = id
			});
		}
	}
}
