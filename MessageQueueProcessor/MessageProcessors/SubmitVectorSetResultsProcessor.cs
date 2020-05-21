using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Internal.TaskQueue;
using NIST.CVP.Libraries.Internal.TaskQueue.Services;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;

namespace MessageQueueProcessor.MessageProcessors
{
	public class SubmitVectorSetResultsProcessor : IMessageProcessor
	{
		private readonly IVectorSetService _vectorSetService;
		private readonly ITaskQueueService _taskQueueService;
		private readonly ITestSessionService _testSessionService;

		public SubmitVectorSetResultsProcessor(IVectorSetService vectorSetService, ITaskQueueService taskQueueService, ITestSessionService testSessionService)
		{
			_vectorSetService = vectorSetService;
			_taskQueueService = taskQueueService;
			_testSessionService = testSessionService;
		}

		public Result Process(Message message)
		{
			//Get the payload so we can get what we need
			VectorSetSubmissionPayload submitResultsPayload = JsonSerializer.Deserialize<VectorSetSubmissionPayload>(message.Payload);

			//Check the status of the vector set, make sure it is processed
			var vectorSet = _vectorSetService.GetVectorSet(submitResultsPayload.VectorSetID);

			if (vectorSet == null)
			{
				return new Result("Vector set does not exist");
			}

			if (vectorSet.Status != VectorSetStatus.Processed)
			{
				return new Result("Vector set must be in Processed status to submit results");
			}

			//Update the test session to show it was touched
			_testSessionService.KeepAlive(_testSessionService.GetTestSessionIDFromVectorSet(submitResultsPayload.VectorSetID));
			
			if (_testSessionService.GetStatus(vectorSet.TestSessionID) == TestSessionStatus.Expired)
			{
				return new Result("Test session that vector set belongs to is Expired");
			}

			//Update the vector set status to show we've received their results
			Result result = _vectorSetService.UpdateStatus(submitResultsPayload.VectorSetID, VectorSetStatus.KATReceived);

			if (result.IsSuccess)
			{
				//Update the submitted results
				result = _vectorSetService.InsertSubmittedAnswers(submitResultsPayload.VectorSetID, JsonSerializer.Serialize(submitResultsPayload));

				if (result.IsSuccess)
				{
					//Add to the task queue
					result = _taskQueueService.AddValidationTask(new ValidationTask
					{
						VectorSetID = submitResultsPayload.VectorSetID,
						ShowExpected = submitResultsPayload.ShowExpected
					});
				}

				if (!result.IsSuccess)
				{
					//Update the status to reflect the error, including the error message 
					_vectorSetService.RecordError(submitResultsPayload.VectorSetID, result.ErrorMessage);    //TODO - figure out if these errors ever get exposed to the end users, as it would be bad to expose the text
				}
			}

			return result;
		}
	}
}
