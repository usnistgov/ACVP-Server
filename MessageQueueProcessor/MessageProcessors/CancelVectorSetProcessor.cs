using System.Text.Json;
using ACVPCore.Results;
using ACVPCore.Services;
using MessageQueueProcessor.MessagePayloads;

namespace MessageQueueProcessor.MessageProcessors
{
	public class CancelVectorSetProcessor : IMessageProcessor
	{
		private readonly IVectorSetService _vectorSetService;

		public CancelVectorSetProcessor(IVectorSetService vectorSetService)
		{
			_vectorSetService = vectorSetService;
		}

		public Result Process(Message message)
		{
			//Get the payload so we can get the test session id
			CancelPayload cancelPayload = JsonSerializer.Deserialize<CancelPayload>(message.Payload);

			//Cancel the test session
			return _vectorSetService.Cancel(cancelPayload.VectorSetID);
		}
	}
}
