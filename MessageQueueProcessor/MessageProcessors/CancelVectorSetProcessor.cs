using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Internal.TaskQueue.Services;
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
		private readonly ITaskQueueService _taskQueueService;

		public CancelVectorSetProcessor(IVectorSetService vectorSetService, ITestSessionService testSessionService, ITaskQueueService taskQueueService)
		{
			_vectorSetService = vectorSetService;
			_testSessionService = testSessionService;
			_taskQueueService = taskQueueService;
		}

		public Result Process(Message message)
		{
			//Get the payload so we can get the vector set id
			CancelPayload cancelPayload = JsonSerializer.Deserialize<CancelPayload>(message.Payload);

			//Update the test session to show it was touched
			_testSessionService.KeepAlive(cancelPayload.TestSessionID);
			
			if (_vectorSetService.GetVectorSet(cancelPayload.VectorSetID).Status == VectorSetStatus.Cancelled)
			{
				return new Result("Vector set not in a valid state for cancellation");
			}

			if (!_testSessionService.GetStatus(cancelPayload.TestSessionID).In(TestSessionStatus.Failed, TestSessionStatus.Passed, TestSessionStatus.PendingEvaluation))
			{
				return new Result("Vector set cannot be cancelled due to state of Test Session");
			}

			//Cancel the vector set
			var result = _vectorSetService.Cancel(cancelPayload.VectorSetID);

			if (result.IsSuccess)
			{
				//Clear any pending tasks in the task queue for this vector set
				result = _taskQueueService.DeletePendingTasksForVectorSet(cancelPayload.VectorSetID);
			}

			return result;
		}
	}
}
