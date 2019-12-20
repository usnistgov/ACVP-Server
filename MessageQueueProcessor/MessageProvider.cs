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

			var data = db.Query("common.MessageQueueGetNextForDotNet").FirstOrDefault();

			return data == null ? null : new Message
			{
				ID = data.ID,
				MessageType = (MessageType)data.MessageType,
				Payload = data.Payload
			};
		}

		public void MarkForJavaProcessor(Guid id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("common.MessageQueueMarkForJava @0", id);
		}

		public void DeleteMessage(Guid id)
		{
			var db = new MightyOrm(_acvpConnectionString);

			db.Execute("common.MessageQueueDelete @0", id);
		}
	}
}
