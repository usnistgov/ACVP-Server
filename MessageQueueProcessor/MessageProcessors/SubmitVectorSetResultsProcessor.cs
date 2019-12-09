using ACVPCore.Services;
using MessageQueueProcessor.MessagePayloads;
using System.Text.Json;

namespace MessageQueueProcessor.MessageProcessors
{
	public class SubmitVectorSetResultsProcessor : IMessageProcessor
	{
		private IVectorSetService _vectorSetService;

		public SubmitVectorSetResultsProcessor(IVectorSetService vectorSetService)
		{
			_vectorSetService = vectorSetService;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get what we need
			SubmitResultsPayload submitResultsPayload = JsonSerializer.Deserialize<SubmitResultsPayload>(message.Payload);

			//Update the submitted results
			_vectorSetService.UpdateSubmittedResults(submitResultsPayload.VectorSetID, submitResultsPayload.Results);
		}
	}
}
