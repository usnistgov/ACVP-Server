using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;

namespace MessageQueueProcessor.MessageProcessors
{
	public class TestSessionKeepAliveProcessor : IMessageProcessor
	{
		private readonly ITestSessionService _testSessionService;

		public TestSessionKeepAliveProcessor(ITestSessionService testSessionService)
		{
			_testSessionService = testSessionService;
		}

		public Result Process(Message message)
		{
			//Get the payload so we can get the test session id
			TestSessionKeepAlivePayload testSessionKeepAlivePayload = JsonSerializer.Deserialize<TestSessionKeepAlivePayload>(message.Payload);

			//Update the test session to show it was touched
			_testSessionService.KeepAlive(testSessionKeepAlivePayload.TestSessionID);

			return new Result();
		}
	}
}
