using System;
using CVP.DatabaseInterface;
using Mighty;

namespace NIST.CVP.MessageQueue.Providers
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
	}
}
