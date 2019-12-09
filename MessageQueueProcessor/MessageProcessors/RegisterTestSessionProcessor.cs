using ACVPCore.Services;
using MessageQueueProcessor.MessagePayloads;
using System.Text.Json;

namespace MessageQueueProcessor.MessageProcessors
{
	public class RegisterTestSessionProcessor : IMessageProcessor
	{
		private ITestSessionService _testSessionService;
		private IVectorSetService _vectorSetService;
		private ITaskQueueService _taskQueueService;

		public RegisterTestSessionProcessor(ITestSessionService testSessionService, IVectorSetService vectorSetService, ITaskQueueService taskQueueService)
		{
			_testSessionService = testSessionService;
			_vectorSetService = vectorSetService;
			_taskQueueService = taskQueueService;
		}

		public void Process(Message message)
		{
			//Deserialize the payload
			RegisterTestSessionPayload registerTestSessionPayload = JsonSerializer.Deserialize<RegisterTestSessionPayload>(message.Payload);

			//Add the test session	
			_testSessionService.Create(registerTestSessionPayload.TestSessionID, registerTestSessionPayload.ACVVersion, "1.0", registerTestSessionPayload.IsSample, registerTestSessionPayload.UserID);

			//Loop through the vector set registrations
			foreach (VectorSetRegistration vectorSetRegistration in registerTestSessionPayload.VectorSetRegistrations)
			{
				//Create the vector set - this actually inserts the Vector Set record and an expected results record with the capabilities
				_vectorSetService.Create(vectorSetRegistration.VectorSetID, registerTestSessionPayload.TestSessionID, "1.0", vectorSetRegistration.AlgorithmID, vectorSetRegistration.Capabilities.ToString());

				//Add to the task queue
				_taskQueueService.AddGenerationTask(new ACVPCore.GenerationTask
				{
					IsSample = registerTestSessionPayload.IsSample,
					VectorSetID = vectorSetRegistration.VectorSetID
				});
			}

		}
	}
}
