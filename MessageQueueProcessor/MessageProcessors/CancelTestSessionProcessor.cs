using System.Text.Json;
using ACVPCore.Services;
using MessageQueueProcessor.MessagePayloads;
using NIST.CVP.Results;

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
