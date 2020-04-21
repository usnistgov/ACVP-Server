using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Internal.MessageQueue.MessagePayloads;
using NIST.CVP.Libraries.Shared.Results;

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
