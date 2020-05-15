using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Internal.MessageQueue.MessagePayloads;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;

namespace MessageQueueProcessor.MessageProcessors
{
	public class CancelTestSessionProcessor : IMessageProcessor
	{
		private readonly ITestSessionService _testSessionService;

		public CancelTestSessionProcessor(ITestSessionService testSessionService)
		{
			_testSessionService = testSessionService;
		}

		public Result Process(Message message)
		{
			//Get the payload so we can get the test session id
			CancelPayload cancelPayload = JsonSerializer.Deserialize<CancelPayload>(message.Payload);

			//Cancel the test session
			return _testSessionService.Cancel(cancelPayload.TestSessionID);
		}
	}
}
