using ACVPCore.Results;
using ACVPCore.Services;
using MessageQueueProcessor.MessagePayloads;
using System.Text.Json;

namespace MessageQueueProcessor.MessageProcessors
{
	public class RegisterTestSessionProcessor : IMessageProcessor
	{
		private readonly ITestSessionService _testSessionService;
		private readonly IVectorSetService _vectorSetService;
		private readonly ITaskQueueService _taskQueueService;

		public RegisterTestSessionProcessor(ITestSessionService testSessionService, IVectorSetService vectorSetService, ITaskQueueService taskQueueService)
		{
			_testSessionService = testSessionService;
			_vectorSetService = vectorSetService;
			_taskQueueService = taskQueueService;
		}

		public Result Process(Message message)
		{
			//Deserialize the payload
			RegisterTestSessionPayload registerTestSessionPayload = JsonSerializer.Deserialize<RegisterTestSessionPayload>(message.Payload);

			//Add the test session	
			Result result = _testSessionService.Create(registerTestSessionPayload.TestSessionID, registerTestSessionPayload.ACVVersion, "1.0", registerTestSessionPayload.IsSample, registerTestSessionPayload.UserID);

			if (result.IsSuccess)
			{
				//Loop through the vector set registrations
				foreach (VectorSetRegistration vectorSetRegistration in registerTestSessionPayload.VectorSetRegistrations)
				{
					//Create the vector set - this actually inserts the Vector Set record and an expected results record with the capabilities
					Result vectorSetResult = _vectorSetService.Create(vectorSetRegistration.VectorSetID, registerTestSessionPayload.TestSessionID, "1.0", vectorSetRegistration.AlgorithmID, vectorSetRegistration.Capabilities.ToString());		//TODO - this generator version can't be hardcoded...

					if (vectorSetResult.IsSuccess)
					{
						//Add to the task queue
						vectorSetResult = _taskQueueService.AddGenerationTask(new ACVPCore.GenerationTask
						{
							IsSample = registerTestSessionPayload.IsSample,
							VectorSetID = vectorSetRegistration.VectorSetID
						});
					}
					
					//If anything with this vector set failed, log the error, but don't stop processing other vector sets
					if (!vectorSetResult.IsSuccess)
					{
						//Update the status to reflect the error, including the error message 
						_vectorSetService.RecordError(vectorSetRegistration.VectorSetID, vectorSetResult.ErrorMessage);	//TODO - figure out if these errors ever get exposed to the end users, as it would be bad to expose the text
					}
				}
			}

			return result;
		}
	}
}
