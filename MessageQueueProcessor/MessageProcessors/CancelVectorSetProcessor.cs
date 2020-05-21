using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
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
			//Get the payload so we can get the test session id
			CancelPayload cancelPayload = JsonSerializer.Deserialize<CancelPayload>(message.Payload);

			if (_vectorSetService.GetVectorSet(cancelPayload.VectorSetID).Status == VectorSetStatus.Cancelled)
			{
				return new Result("Vector set not in a valid state for cancellation");
			}

			if (!_testSessionService.GetStatus(cancelPayload.TestSessionID).In(TestSessionStatus.Failed, TestSessionStatus.Passed, TestSessionStatus.PendingEvaluation))
			{
				return new Result("Vector set cannot be cancelled due to state of Test Session");
			}

			//Cancel the vector set
			return _vectorSetService.Cancel(cancelPayload.VectorSetID);
		}
	}
}
