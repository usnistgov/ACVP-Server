using ACVPCore;
using ACVPCore.Results;
using ACVPCore.Services;
using MessageQueueProcessor.MessagePayloads;
using System.Text.Json;

namespace MessageQueueProcessor.MessageProcessors
{
	public class SubmitVectorSetResultsProcessor : IMessageProcessor
	{
		private readonly IVectorSetService _vectorSetService;
		private readonly ITaskQueueService _taskQueueService;

		public SubmitVectorSetResultsProcessor(IVectorSetService vectorSetService, ITaskQueueService taskQueueService)
		{
			_vectorSetService = vectorSetService;
			_taskQueueService = taskQueueService;
		}

		public Result Process(Message message)
		{
			//Get the payload so we can get what we need
			SubmitResultsPayload submitResultsPayload = JsonSerializer.Deserialize<SubmitResultsPayload>(message.Payload);

			//Update the submitted results
			Result result = _vectorSetService.UpdateSubmittedResults(submitResultsPayload.VectorSetID, submitResultsPayload.Results);

			if (result.IsSuccess)
			{
				//Add to the task queue
				result = _taskQueueService.AddValidationTask(new ValidationTask
				{
					VectorSetID = submitResultsPayload.VectorSetID
				});
			}

			if (!result.IsSuccess)
			{
				//Update the status to reflect the error, including the error message 
				_vectorSetService.RecordError(submitResultsPayload.VectorSetID, result.ErrorMessage);    //TODO - figure out if these errors ever get exposed to the end users, as it would be bad to expose the text
			}

			return result;
		}
	}
}
