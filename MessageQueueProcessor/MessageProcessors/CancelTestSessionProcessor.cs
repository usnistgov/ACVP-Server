using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
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

			//Update the test session to show it was touched - a little useless, but do it for consistency
			_testSessionService.KeepAlive(cancelPayload.TestSessionID);
			
			//Check that the test session is in a status where it can be cancelled
			if (!_testSessionService.GetStatus(cancelPayload.TestSessionID).In(TestSessionStatus.Failed, TestSessionStatus.Passed, TestSessionStatus.PendingEvaluation))
			{
				return new Result("Test session not in a valid state for cancellation");
			}

			//Cancel the test session
			return _testSessionService.Cancel(cancelPayload.TestSessionID);
		}
	}
}
