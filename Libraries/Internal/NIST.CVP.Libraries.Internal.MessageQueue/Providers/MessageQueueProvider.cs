using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.ExtensionMethods;

namespace NIST.CVP.Libraries.Internal.MessageQueue.Providers
{
	public class MessageQueueProvider : IMessageQueueProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<MessageQueueProvider> _logger;

		public MessageQueueProvider(IConnectionStringFactory connectionStringFactory, ILogger<MessageQueueProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Message GetNextMessage()
		{
			try
			{
				var db = new MightyOrm<Message>(_acvpConnectionString);

				return db.SingleFromProcedure("common.MessageQueueGetNext");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return null;
			}
		}

		public void UpdateStatus(Guid id, MessageStatus messageStatus)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				db.ExecuteProcedure("common.MessageQueueUpdateStatus", inParams: new
				{
					MessageId = id,
					StatusId = messageStatus
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}
		}

		public void DeleteMessage(Guid id)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				db.ExecuteProcedure("common.MessageQueueDelete", inParams: new
				{
					MessageId = id
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}
		}

		public List<MessageQueueItem> List()
		{
			try
			{
				var db = new MightyOrm<MessageQueueItem>(_acvpConnectionString);

				return db.QueryFromProcedure("common.MessageQueueList").ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return new List<MessageQueueItem>();
			}
		}

		public string GetMessagePayload(Guid id)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);
				return db.ScalarFromProcedure("common.MessageGetPayload", inParams: new
				{
					MessageId = id
				}).ToString();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return null;
			}
		}
	}
}
