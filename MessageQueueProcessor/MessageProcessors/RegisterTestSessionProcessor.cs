using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Internal.MessageQueue.MessagePayloads;
using NIST.CVP.Libraries.Shared.Results;
using NIST.CVP.Libraries.Internal.TaskQueue;
using NIST.CVP.Libraries.Internal.TaskQueue.Services;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;

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
			var registerTestSessionPayload = JsonSerializer.Deserialize<TestSessionRegisterPayload>(message.Payload);

			//Add the test session	
			Result result = _testSessionService.Create(registerTestSessionPayload.ID, registerTestSessionPayload.AcvVersion, "1.0", registerTestSessionPayload.IsSample, message.UserID);

			if (result.IsSuccess)
			{
				//Loop through the vector set registrations
				foreach (var vectorSetRegistration in registerTestSessionPayload.Algorithms)
				{
					//Create the vector set - this actually inserts the Vector Set record and the capabilities
					Result vectorSetResult = _vectorSetService.Create(
						vectorSetRegistration.VsID, 
						registerTestSessionPayload.ID, 
						"1.0", //HACK - this generator version shouldn't be hardcoded, but it was in Java, and it isn't clear what it was for
						vectorSetRegistration.AlgorithmId, 
						JsonSerializer.Serialize(vectorSetRegistration));		

					if (vectorSetResult.IsSuccess)
					{
						//Add to the task queue
						vectorSetResult = _taskQueueService.AddGenerationTask(new GenerationTask
						{
							IsSample = registerTestSessionPayload.IsSample,
							VectorSetID = vectorSetRegistration.VsID
						});
					}
					
					//If anything with this vector set failed, log the error, but don't stop processing other vector sets
					if (!vectorSetResult.IsSuccess)
					{
						//Update the status to reflect the error, including the error message 
						_vectorSetService.RecordError(vectorSetRegistration.VsID, vectorSetResult.ErrorMessage);	//TODO - figure out if these errors ever get exposed to the end users, as it would be bad to expose the text
					}
				}
			}

			return result;
		}
	}
}
