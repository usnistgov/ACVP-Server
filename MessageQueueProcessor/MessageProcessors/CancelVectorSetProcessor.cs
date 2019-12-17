using ACVPCore.Services;
using MessageQueueProcessor.MessagePayloads;
using System.Text.Json;

namespace MessageQueueProcessor.MessageProcessors
{
	public class CancelVectorSetProcessor : IMessageProcessor
	{
		private readonly IVectorSetService _vectorSetService;

		public CancelVectorSetProcessor(IVectorSetService vectorSetService)
		{
			_vectorSetService = vectorSetService;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get the test session id
			CancelPayload cancelPayload = JsonSerializer.Deserialize<CancelPayload>(message.Payload);

			//Cancel the test session
			_vectorSetService.Cancel(cancelPayload.VectorSetID);
		}
	}
}
