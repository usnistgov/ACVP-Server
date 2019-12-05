using System.Text.Json;
using ACVPCore.Services;
using ACVPWorkflow.Services;
using MessageQueueProcessor.MessagePayloads;

namespace MessageQueueProcessor.MessageProcessors
{
	public class DeleteDependencyProcessor : IMessageProcessor
	{
		private IDependencyService _dependencyService;
		private IWorkflowService _workflowService;

		public DeleteDependencyProcessor(IDependencyService dependencyService, IWorkflowService workflowService)
		{
			_dependencyService = dependencyService;
			_workflowService = workflowService;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get the Json
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Deserialize the JSON to get the ID, as that's all that's there
			DeletePayload deletePayload = JsonSerializer.Deserialize<DeletePayload>(requestPayload.Json.ToString());

			//Create the workflow item
			_workflowService.CreateDependencyDelete(deletePayload.ID, null);



			//TODO - Autoapprove logic here
			if (false)
			{
				//Delete that dependency
				_dependencyService.Delete(deletePayload.ID);
			}

			//TODO - something about updating a request?

		}
	}
}
