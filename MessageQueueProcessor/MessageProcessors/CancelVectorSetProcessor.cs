using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;

namespace MessageQueueProcessor.MessageProcessors
{
	public class CancelVectorSetProcessor : IMessageProcessor
	{
		private readonly IVectorSetService _vectorSetService;
		private readonly ITestSessionService _testSessionService;

		public CancelVectorSetProcessor(IVectorSetService vectorSetService, ITestSessionService testSessionService)
		{
			_vectorSetService = vectorSetService;
			_testSessionService = testSessionService;
		}

		public Result Process(Message message)
		{
			//Get the payload so we can get the vector set id
			CancelPayload cancelPayload = JsonSerializer.Deserialize<CancelPayload>(message.Payload);

			//Update the test session to show it was touched
			_testSessionService.KeepAlive(cancelPayload.TestSessionID);

			//Cancel the vector set
			return _vectorSetService.Cancel(cancelPayload.VectorSetID);
		}
	}
}
