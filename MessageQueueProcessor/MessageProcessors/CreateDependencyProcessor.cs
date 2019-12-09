using ACVPCore.Services;
using MessageQueueProcessor.MessagePayloads;
using System.Text.Json;

namespace MessageQueueProcessor.MessageProcessors
{
	public class CreateDependencyProcessor : IMessageProcessor
	{
		private IVectorSetService _vectorSetService;

		public CreateDependencyProcessor(IVectorSetService vectorSetService)
		{
			_vectorSetService = vectorSetService;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get the Json
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Deserialize the JSON into a Dependency object

			//Create that object


		}
	}
}
