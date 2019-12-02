using ACVPCore.Services;
using MessageQueueProcessor.MessageProcessors;

namespace MessageQueueProcessor
{
	public class MessageProcessorFactory : IMessageProcessorFactory
	{
		private ITestSessionService _testSessionService;
		private IVectorSetService _vectorSetService;
		private ITaskQueueService _taskQueueService;
		private IDependencyService _dependencyService;

		public MessageProcessorFactory(ITestSessionService testSessionService, IVectorSetService vectorSetService, ITaskQueueService taskQueueService, IDependencyService dependencyService)
		{
			_testSessionService = testSessionService;
			_vectorSetService = vectorSetService;
			_taskQueueService = taskQueueService;
			_dependencyService = dependencyService;
		}

		public IMessageProcessor GetMessageProcessor(MessageAction action)
		{
			return action switch
			{
				MessageAction.CreateDependency => null,
				MessageAction.UpdateDependency => null,
				MessageAction.DeleteDependency => null,		//new DeleteDependencyProcessor(_dependencyService),
				MessageAction.CreateImplementation => null,
				MessageAction.UpdateImplementation => null,
				MessageAction.DeleteImplementation => null,
				MessageAction.CreateOE => null,
				MessageAction.UpdateOE => null,
				MessageAction.DeleteOE => null,
				MessageAction.CreatePerson => null,
				MessageAction.UpdatePerson => null,
				MessageAction.DeletePerson => null,
				MessageAction.CreateVendor => null,
				MessageAction.UpdateVendor => null,
				MessageAction.DeleteVendor => null,
				MessageAction.RegisterTestSession => new RegisterTestSessionProcessor(_testSessionService, _vectorSetService, _taskQueueService),
				MessageAction.CancelTestSession => new CancelTestSessionProcessor(_testSessionService),
				MessageAction.CertifyTestSession => null,
				MessageAction.SubmitVectorSetResults => null,                   //new SubmitVectorSetResultsProcessor(_vectorSetService);
				MessageAction.CancelVectorSet => new CancelVectorSetProcessor(_vectorSetService),
				_ => null
			};
		}
	}
}
