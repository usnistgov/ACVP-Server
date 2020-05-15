using MessageQueueProcessor.MessageProcessors;
using Microsoft.Extensions.Options;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Services;
using NIST.CVP.Libraries.Internal.TaskQueue.Services;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace MessageQueueProcessor
{
	public class MessageProcessorFactory : IMessageProcessorFactory
	{
		private readonly ITestSessionService _testSessionService;
		private readonly IVectorSetService _vectorSetService;
		private readonly ITaskQueueService _taskQueueService;
		private readonly IWorkflowService _workflowService;
		private readonly MessageQueueProcessorConfig _messageQueueProcessorConfig;
		private readonly IWorkflowItemPayloadFactory _workflowItemPayloadFactory;

		public MessageProcessorFactory(ITestSessionService testSessionService, IVectorSetService vectorSetService, ITaskQueueService taskQueueService, IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, IWorkflowItemPayloadFactory workflowItemPayloadFactory, IOptions<MessageQueueProcessorConfig> messageQueueProcessorConfig)
		{
			_testSessionService = testSessionService;
			_vectorSetService = vectorSetService;
			_taskQueueService = taskQueueService;
			_workflowService = workflowService;
			_workflowItemPayloadFactory = workflowItemPayloadFactory;
			_messageQueueProcessorConfig = messageQueueProcessorConfig.Value;
		}

		public IMessageProcessor GetMessageProcessor(APIAction action)
		{
			return action switch
			{
				APIAction.CreateDependency => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.UpdateDependency => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.DeleteDependency => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.CreateImplementation => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.UpdateImplementation => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.DeleteImplementation => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.CreateOE => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.UpdateOE => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.DeleteOE => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.CreatePerson => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.UpdatePerson => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.DeletePerson => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.CreateVendor => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.UpdateVendor => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.DeleteVendor => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				APIAction.RegisterTestSession => new RegisterTestSessionProcessor(_testSessionService, _vectorSetService, _taskQueueService),
				APIAction.CancelVectorSet => new CancelVectorSetProcessor(_vectorSetService),
				APIAction.CancelTestSession => new CancelTestSessionProcessor(_testSessionService),
				APIAction.SubmitVectorSetResults => new SubmitVectorSetResultsProcessor(_vectorSetService, _taskQueueService),
				APIAction.ResubmitVectorSetResults => new ResubmitVectorSetResultsProcessor(_vectorSetService, _taskQueueService, _messageQueueProcessorConfig),
				APIAction.CertifyTestSession => new CertifyTestSessionProcessor(_testSessionService, _workflowService, _workflowItemPayloadFactory, _messageQueueProcessorConfig),
				_ => null
			};
		}
	}
}
