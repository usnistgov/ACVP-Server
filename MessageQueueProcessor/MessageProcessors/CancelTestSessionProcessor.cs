using ACVPCore.Services;
using MessageQueueProcessor.MessagePayloads;
using System.Text.Json;

namespace MessageQueueProcessor.MessageProcessors
{
	public class CancelTestSessionProcessor : IMessageProcessor
	{
		private readonly ITestSessionService _testSessionService;

		public CancelTestSessionProcessor(ITestSessionService testSessionService)
		{
			_testSessionService = testSessionService;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get the test session id
			CancelPayload cancelPayload = JsonSerializer.Deserialize<CancelPayload>(message.Payload);

			//Cancel the test session
			_testSessionService.Cancel(cancelPayload.TestSessionID);
		}
	}
}
