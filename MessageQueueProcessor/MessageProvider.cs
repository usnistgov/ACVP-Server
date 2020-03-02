using System;
using System.Linq;
using CVP.DatabaseInterface;
using Mighty;

namespace MessageQueueProcessor
{
	public class MessageProvider : IMessageProvider
	{
		private readonly string _acvpConnectionString;

		public MessageProvider(IConnectionStringFactory connectionStringFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
		}

		public Message GetNextMessage()
		{
			var db = new MightyOrm(_acvpConnectionString);

			var data = db.QueryFromProcedure("common.MessageQueueGetNext").FirstOrDefault();

			return data == null ? null : new Message
			{
				ID = data.ID,
				MessageType = (MessageType)data.MessageType,
				Payload = data.Payload
			};
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
	}
}
