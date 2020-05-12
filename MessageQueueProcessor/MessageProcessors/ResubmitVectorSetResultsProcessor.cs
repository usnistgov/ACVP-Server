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
	public class ResubmitVectorSetResultsProcessor : IMessageProcessor
	{
		private readonly IVectorSetService _vectorSetService;
		private readonly ITaskQueueService _taskQueueService;
		private readonly MessageQueueProcessorConfig _messageQueueProcessorConfig;

		public ResubmitVectorSetResultsProcessor(IVectorSetService vectorSetService, ITaskQueueService taskQueueService, MessageQueueProcessorConfig messageQueueProcessorConfig)
		{
			_vectorSetService = vectorSetService;
			_taskQueueService = taskQueueService;
			_messageQueueProcessorConfig = messageQueueProcessorConfig;
		}

		public Result Process(Message message)
		{
			if (!_messageQueueProcessorConfig.AllowResubmission)
			{
				return new Result("Resubmission of vector set results not permitted");
			}

			//Get the payload so we can get what we need
			VectorSetSubmissionPayload submitResultsPayload = JsonSerializer.Deserialize<VectorSetSubmissionPayload>(message.Payload);

			//Check the status of the vector set, make sure it is failed
			var vectorSet = _vectorSetService.GetVectorSet(submitResultsPayload.VectorSetID);

			if (vectorSet == null)
			{
				return new Result("Vector set does not exist");
			}

			if (vectorSet.Status != VectorSetStatus.Failed)
			{
				return new Result("Vector set must be in Failed status to resubmit results");
			}

			//Update the vector set status to show we've received their results
			Result result = _vectorSetService.UpdateStatus(submitResultsPayload.VectorSetID, VectorSetStatus.KATReceived);

			if (result.IsSuccess)
			{
				//Delete the submitted answers, validation results, and errors, but only continue if all successful
				if (_vectorSetService.RemoveVectorFileJson(submitResultsPayload.VectorSetID, VectorSetJsonFileTypes.SubmittedAnswers).IsSuccess
					&& _vectorSetService.RemoveVectorFileJson(submitResultsPayload.VectorSetID, VectorSetJsonFileTypes.Validation).IsSuccess
					&&_vectorSetService.RemoveVectorFileJson(submitResultsPayload.VectorSetID, VectorSetJsonFileTypes.Error).IsSuccess)
				{
					//Insert the new results
					result = _vectorSetService.InsertSubmittedAnswers(submitResultsPayload.VectorSetID, JsonSerializer.Serialize(submitResultsPayload));

					if (result.IsSuccess)
					{
						//Add to the task queue
						result = _taskQueueService.AddValidationTask(new ValidationTask
						{
							VectorSetID = submitResultsPayload.VectorSetID
						});
					}
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
