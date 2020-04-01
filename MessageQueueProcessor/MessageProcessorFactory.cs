using ACVPCore.Services;
using ACVPWorkflow;
using ACVPWorkflow.Services;
using MessageQueueProcessor.MessageProcessors;
using Microsoft.Extensions.Configuration;
using NIST.CVP.TaskQueue.Services;

namespace MessageQueueProcessor
{
	public class MessageProcessorFactory : IMessageProcessorFactory
	{
		private readonly ITestSessionService _testSessionService;
		private readonly IVectorSetService _vectorSetService;
		private readonly ITaskQueueService _taskQueueService;
		private readonly IWorkflowService _workflowService;
		private readonly IConfigurationSection _autoApproveConfiguration;
		private readonly IWorkflowItemPayloadFactory _workflowItemPayloadFactory;

		public MessageProcessorFactory(ITestSessionService testSessionService, IVectorSetService vectorSetService, ITaskQueueService taskQueueService, IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, IWorkflowItemPayloadFactory workflowItemPayloadFactory, IConfiguration configuration)
		{
			_testSessionService = testSessionService;
			_vectorSetService = vectorSetService;
			_taskQueueService = taskQueueService;
			_workflowService = workflowService;
			_workflowItemPayloadFactory = workflowItemPayloadFactory;
			_autoApproveConfiguration = configuration.GetSection("MessageQueueProcessor:AutoApprove");
		}

		public IMessageProcessor GetMessageProcessor(APIAction action)
		{
			return action switch
			{
				APIAction.CreateDependency => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.UpdateDependency => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.DeleteDependency => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.CreateImplementation => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.UpdateImplementation => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.DeleteImplementation => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.CreateOE => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.UpdateOE => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.DeleteOE => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.CreatePerson => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.UpdatePerson => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.DeletePerson => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.CreateVendor => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.UpdateVendor => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.DeleteVendor => new RequestProcessor(_workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				APIAction.RegisterTestSession => new RegisterTestSessionProcessor(_testSessionService, _vectorSetService, _taskQueueService),
				APIAction.CancelVectorSet => new CancelVectorSetProcessor(_vectorSetService),
				APIAction.CancelTestSession => new CancelTestSessionProcessor(_testSessionService),
				APIAction.SubmitVectorSetResults => new SubmitVectorSetResultsProcessor(_vectorSetService, _taskQueueService),
				APIAction.CertifyTestSession => new CertifyTestSessionProcessor(_testSessionService, _workflowService, _workflowItemPayloadFactory, _autoApproveConfiguration),
				_ => null
			};
		}
	}
}
